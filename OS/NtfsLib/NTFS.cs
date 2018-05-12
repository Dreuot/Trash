using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    public class NTFS
    {
        public SafeFileHandle Drive { get; set; }
        public BPB BPB { get; set; }
        public MFT MFT { get; set; }

        public NTFS(string drive)
        {
            drive = "\\\\.\\" + drive;

            Drive = HD_API.CreateFile(
                drive,
                HD_API.GENERIC_READ,
                HD_API.FILE_SHARE_READ | HD_API.FILE_SHARE_WRITE,
                IntPtr.Zero,
                HD_API.OPEN_EXISTING,
                0,
                IntPtr.Zero
                );

            BPB = new BPB(Drive);
            MFT = GetFirstMFT();
        }

        private unsafe byte[] _ReadSector(int sectorNum)
        {
            byte[] bytes = new byte[BPB.BYTE_IN_SECTOR]; // сектор в виде одномерного массива байтов
            IntPtr BytesRead = IntPtr.Zero;
            ulong pointer = (ulong)sectorNum * (ulong)BPB.BYTE_IN_SECTOR;
            int hight = (int)(pointer >> 32);
            HD_API.SetFilePointer(Drive, (int)(pointer & 0xffffffff), out hight, HD_API.EMoveMethod.Begin);

            fixed (byte* ptr = bytes)
            {
                HD_API.ReadFile(Drive, ptr, BPB.BYTE_IN_SECTOR, BytesRead, IntPtr.Zero); // Считывание сектора
            };

            HD_API.SetFilePointer(Drive, 0, out hight, HD_API.EMoveMethod.Begin);

            return bytes;
        }

        public byte[] ReadSector(int num)
        {
            if ((ulong)num >= BPB.SecCount)
                throw new ArgumentException($"Слишком большой номер сектора {num}. Максимальный номер сектора {BPB.SecCount}");

            return _ReadSector(num);
        }

        public string[] SectorToHex(byte[] sector)
        {
            string[] SectorHex = new string[sector.Length];

            for (int i = 0; i < sector.Length; i++)
                SectorHex[i] = BitConverter.ToString(new byte[] { sector[i] }); // Преобразование к шестнадцатиричному виду

            return SectorHex;
        }

        public byte[] ReadSectorByClusterNum(int num)
        {
            int sectorNum = num * BPB.SectorPerCluster;//* 8;
            if ((ulong)sectorNum >= BPB.SecCount)
            {
                throw new ArgumentException($"Слишком большой номер кластера {num}. Максимальный номер сектора {BPB.SecCount}");
            }

            return _ReadSector(sectorNum);
        }

        public byte[] ReadCluster(int cluster)
        {
            byte[] ByteRecord = new byte[BPB.SectorPerCluster * BPB.BytePerSec];
            for (int i = 0; i < ByteRecord.Length / 512; i++)
            {
                Array.Copy(ReadSector((int)(cluster * BPB.SectorPerCluster) + i), 0, ByteRecord, i * BPB.BytePerSec, BPB.BytePerSec);
            }

            return ByteRecord;
        }

        public MFT GetFirstMFT()
        {
            byte[] ByteRecord = new byte[(int)Math.Pow(2, BPB.ClustersPerMFT * -1)];
            for (int i = 0; i < ByteRecord.Length / 512; i++)
            {
                Array.Copy(ReadSector((int)(BPB.FirstClusterMFT * BPB.SectorPerCluster) + i), 0, ByteRecord, i * BPB.BytePerSec, BPB.BytePerSec);
            }

            return new MFT(ByteRecord, this);
        }

        public MFT GetMftRecord(int indexMFT)
        {
            int mftSize = (int)Math.Pow(2, BPB.ClustersPerMFT * -1);
            int recordInCluster = BPB.SectorPerCluster * BPB.BytePerSec / mftSize;
            int sector = FindSector(indexMFT);

            byte[] ByteRecord = new byte[mftSize];
            for (int i = 0; i < ByteRecord.Length / 512; i++)
            {
                Array.Copy(ReadSector(sector + i), 0, ByteRecord, i * BPB.BytePerSec, BPB.BytePerSec);
            }

            return new MFT(ByteRecord, this);
        }

        private int FindSector(int indexMFT)
        {
            int mftSize = (int)Math.Pow(2, BPB.ClustersPerMFT * -1);
            int recordInCluster = BPB.SectorPerCluster * BPB.BytePerSec / mftSize;
            Attribute data = MFT.Attributes.Where(n => n.Type == AttributeTypes.AT_DATA).FirstOrDefault();
            int prevMin = 0;
            int prevMax = 0;
            int maxRec = 0;
            int minRec = 0;
            int run = 0;
            for (int i = 0; i < data.NonResident.Clusters.Count; i++)
            {
                minRec = prevMax;
                maxRec = (int)(data.NonResident.Clusters[i].End - data.NonResident.Clusters[i].Start) * recordInCluster + prevMax;
                if (indexMFT < maxRec)
                {
                    run = i;
                    break;
                }

                prevMin = minRec;
                prevMax = maxRec;
            }

            int startSectorOfCluster = (int)data.NonResident.Clusters[run].Start * BPB.SectorPerCluster;
            int recordInRun = indexMFT - minRec;
            int number = startSectorOfCluster + recordInRun * (mftSize / BPB.BytePerSec);

            return number;
        }
    }
}
