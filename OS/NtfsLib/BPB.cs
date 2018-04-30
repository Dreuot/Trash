using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    public class BPB
    {
        public static readonly int BYTE_IN_LINE = 16;
        public static readonly int LINE_IN_SECTOR = 32;
        public static readonly int BYTE_IN_SECTOR = 512;
        public static readonly byte SEC_PER_CLUS = 0x0D;
        public static readonly ushort SEC_PER_TRACK = 0x18;
        public static readonly long START_CLUS_MFT = 0x30;
        public static readonly byte CLUS_ON_RECORD_MFT = 0x40;
        public static readonly byte HEX = 16;

        private byte[] bootSector;

        public string Signature { get; set; }
        public int BytePerSec { get; set; }
        public byte SectorPerCluster { get; set; }
        public byte TypeOfDrive { get; set; }
        public ushort SecPerTrack { get; set; }
        public ushort NumHeads { get; set; }
        public long HiddenSec { get; set; }
        public ulong SecCount { get; set; }
        public ulong FirstClusterMFT { get; set; }
        public ulong FirstClusterMirror { get; set; }
        public int ClustersPerMFT { get; set; }
        public int ClustertPerIndex { get; set; }
        public ulong SerialNumber { get; set; }
        public SafeFileHandle Drive { get; set; }
        public byte[] BootSector
        {
            get
            {
                if (bootSector == null)
                    bootSector = LoadBootSector();

                return bootSector;
            }
            private set
            {
                bootSector = value;
            }
        }

        public BPB(SafeFileHandle drive)
        {
            Drive = drive;

            ReadBPB();
        }

        private void ReadBPB()
        {
            bootSector = LoadBootSector();
            byte[] sector = BootSector;
            Signature = "";
            for (int i = 0; i < 8; i++)
                Signature += (char)sector[0x3 + i];

            BytePerSec = sector[0xB] + (sector[0xC] << 8);
            SectorPerCluster = sector[SEC_PER_CLUS];
            TypeOfDrive = sector[0x15];
            SecPerTrack = sector[0x18];
            NumHeads = sector[0x1A];

            HiddenSec = 0;
            for (int i = 0; i < 4; i++)
                HiddenSec += sector[0x1C + i] << (i * 8);

            SecCount = 0;
            for (int i = 0; i < 8; i++)
                SecCount += (ulong)sector[0x28 + i] << (i * 8);

            FirstClusterMFT = 0;
            for (int i = 0; i < 8; i++)
                FirstClusterMFT += (ulong)sector[i + START_CLUS_MFT] << (8 * i);

            FirstClusterMirror = 0;
            for (int i = 0; i < 8; i++)
                FirstClusterMirror += (ulong)sector[i + START_CLUS_MFT + 8] << (8 * i);

            ClustersPerMFT = (sbyte)sector[0x40];
            ClustertPerIndex = (sbyte)sector[0x44];

            SerialNumber = 0;
            for (int i = 0; i < 8; i++)
                SerialNumber += (ulong)sector[i + 0x48] << (8 * i);
        }

        private unsafe byte[] LoadBootSector()
        {
            byte[] bytes = new byte[BYTE_IN_SECTOR]; // BOOT сектор в виде одномерного массива байтов
            IntPtr BytesRead = IntPtr.Zero;

            fixed (byte* ptr = bytes)
            {
                HD_API.ReadFile(Drive, ptr, BYTE_IN_SECTOR, BytesRead, IntPtr.Zero); // Считывание первого сектора
            };

            return bytes;
       }

        public override string ToString()
        {
            return String.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}\n{9}\n{10}\n{11}\n{12}",
                Signature, BytePerSec, SectorPerCluster, TypeOfDrive, SecPerTrack, NumHeads,
                HiddenSec, SecCount, FirstClusterMFT, FirstClusterMirror, ClustersPerMFT,
                ClustertPerIndex, SerialNumber);
        }
    }
}
