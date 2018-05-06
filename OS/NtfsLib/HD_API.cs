using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;


namespace NtfsLib
{
    class HD_API
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall,
            SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
              string lpFileName,
              uint dwDesiredAccess,
              uint dwShareMode,
              IntPtr SecurityAttributes,
              uint dwCreationDisposition,
              uint dwFlagsAndAttributes,
              IntPtr hTemplateFile
              );

        public enum EMoveMethod : uint
        {
            Begin = 0,
            Current = 1,
            End = 2
        }

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern unsafe uint SetFilePointer(
            [In] SafeFileHandle hFile,
            [In] int lDistanceToMove,
            [Out] int* lpDistanceToMoveHigh,
            [In] EMoveMethod dwMoveMethod);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SetFilePointer(
            [In] SafeFileHandle hFile,
            [In] int lDistanceToMove,
            [Out] out int lpDistanceToMoveHigh,
            [In] EMoveMethod dwMoveMethod);

        [DllImport(@"kernel32.dll", SetLastError = true)]
        public static extern unsafe bool ReadFile(
            SafeFileHandle hFile,      // handle to file
            byte* pBuffer,        // data buffer, should be fixed
            int NumberOfBytesToRead,  // number of bytes to read
            IntPtr pNumberOfBytesRead,  // number of bytes read, provide IntPtr.Zero here
            /*NativeOverlapped**/IntPtr lpOverlapped // should be fixed, if not IntPtr.Zero
            );

        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool CloseHandle(
            System.IntPtr hObject // handle to object
        );

        public const uint GENERIC_READ = 0x80000000;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const uint OPEN_EXISTING = 0x3;
        public const uint IOCTL_DISK_GET_DRIVE_GEOMETRY = 0x70000;

        public static unsafe byte[] ReturnSector(SafeFileHandle drive, int sectorNum)
        {
            byte[] bytes = new byte[BPB.BYTE_IN_SECTOR]; // BOOT сектор в виде одномерного массива байтов
            int distance;
            IntPtr BytesRead = IntPtr.Zero;
            SetFilePointer(drive, sectorNum * BPB.BYTE_IN_SECTOR, out distance, EMoveMethod.Begin);

            fixed (byte* ptr = bytes)
            {
                ReadFile(drive, ptr, BPB.BYTE_IN_SECTOR, BytesRead, IntPtr.Zero); // Считывание первого сектора
            };

            SetFilePointer(drive, 0, out distance, EMoveMethod.Begin);

            return bytes;
        }
    }
}
