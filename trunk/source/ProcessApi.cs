using System.Runtime.InteropServices;
using System.Diagnostics;
using System;

namespace Reanimator
{
    class ProcessApi
    {
        public const uint PROCESS_VM_READ = (0x0010);
        public const uint PROCESS_VM_WRITE = (0x0020);
        public const uint PROCESS_ALL_ACCESS = (0x001F0FFF);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess
        (
            UInt32 dwDesiredAccess,
            Int32 bInheritHandle,
            UInt32 dwProcessId
        );

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory
        (
            IntPtr OpenedHandle,
            IntPtr lpBaseAddress,
            [In, Out] byte[] lpBuffer,
            UInt32 nSize,
            out IntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory
        (
            IntPtr OpenedHandle,
            IntPtr lpBaseAddress,
            [In, Out] byte[] lpBuffer,
            UInt32 nSize,
            out IntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle
        (
            IntPtr hObject
        );

        [DllImport("kernel32.dll")]
        public static extern Int32 GetLastError();
    }
}
