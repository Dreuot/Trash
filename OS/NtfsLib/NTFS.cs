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
            drive = drive.Remove(drive.Length - 1, 1);

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
            MFT = ReturnMFTRecord(0);
        }

        private unsafe byte[] _ReadSector(int sectorNum)
        {
            byte[] bytes = new byte[BPB.BYTE_IN_SECTOR]; // BOOT сектор в виде одномерного массива байтов
            IntPtr BytesRead = IntPtr.Zero;
            HD_API.SetFilePointer(Drive, sectorNum * BPB.BYTE_IN_SECTOR, out int distance, HD_API.EMoveMethod.Begin);

            fixed (byte* ptr = bytes)
            {
                HD_API.ReadFile(Drive, ptr, BPB.BYTE_IN_SECTOR, BytesRead, IntPtr.Zero); // Считывание первого сектора
            };

            HD_API.SetFilePointer(Drive, 0, out distance, HD_API.EMoveMethod.Begin);

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

        public MFT ReturnMFTRecord(int indexMFT)
        {
            byte[] ByteRecord = new byte[(int)Math.Pow(2, BPB.ClustersPerMFT * -1)];
            for (int i = 0; i < ByteRecord.Length / 512; i++)
            {
                Array.Copy(ReadSector((int)(BPB.FirstClusterMFT * BPB.SectorPerCluster) + indexMFT * 2 + i), 0, ByteRecord, i * BPB.BytePerSec, BPB.BytePerSec);
            }

            return new MFT(ByteRecord, this);
        }
    }
}
