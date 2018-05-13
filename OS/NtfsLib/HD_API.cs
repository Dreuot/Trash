using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;


namespace NtfsLib
{
    /// <summary>
    /// WinAPI функции, необходимые для работы с жестким диском на низком уровне
    /// </summary>
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

        /// <summary>
        /// Чтение сектора 
        /// </summary>
        /// <param name="drive">Файл, представляющий диск</param>
        /// <param name="sectorNum">Номер сектора</param>
        /// <returns></returns>
        public static unsafe byte[] ReturnSector(SafeFileHandle drive, int sectorNum) // Чтение сектора под номером sectorNum
        {
            byte[] bytes = new byte[BPB.BYTE_IN_SECTOR]; // сектор в виде одномерного массива байтов
            IntPtr BytesRead = IntPtr.Zero;
            ulong pointer = (ulong)sectorNum * (ulong)BPB.BYTE_IN_SECTOR;
            int hight = (int)(pointer >> 32);
            HD_API.SetFilePointer(drive, (int)(pointer & 0xffffffff), out hight, HD_API.EMoveMethod.Begin);

            fixed (byte* ptr = bytes)
            {
                HD_API.ReadFile(drive, ptr, BPB.BYTE_IN_SECTOR, BytesRead, IntPtr.Zero); // Считывание сектора
            };

            HD_API.SetFilePointer(drive, 0, out hight, HD_API.EMoveMethod.Begin);

            return bytes;
        }
    }
}
