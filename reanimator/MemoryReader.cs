//------------------------------------------------------------------------------
//  <copyright from='2004' to='2005' company='WoWSharp.NET'>
//    Copyright (c) WoWSharp.NET. All Rights Reserved.
//
//    Please look in the accompanying license.htm file for the license that 
//    applies to this source code. (a copy can also be found at: 
//    http://www.wowsharp.net/license.htm)
//  </copyright>
//-------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
using System.Text;

namespace Reanimator
{
    /// <summary>
    /// Summary description for MemoryReader.
    /// </summary>
    public class MemoryReader
    {
        /// <summary>
        /// Constants information can be found in [winnt.h]
        /// </summary>
        private const uint PROCESS_TERMINATE = 0x0001;
        private const uint PROCESS_CREATE_THREAD = 0x0002;
        private const uint PROCESS_SET_SESSIONID = 0x0004;
        private const uint PROCESS_VM_OPERATION = 0x0008;
        private const uint PROCESS_VM_READ = 0x0010;
        private const uint PROCESS_VM_WRITE = 0x0020;
        private const uint PROCESS_DUP_HANDLE = 0x0040;
        private const uint PROCESS_CREATE_PROCESS = 0x0080;
        private const uint PROCESS_SET_QUOTA = 0x0100;
        private const uint PROCESS_SET_INFORMATION = 0x0200;
        private const uint PROCESS_QUERY_INFORMATION = 0x0400;

        private const uint TH32CS_SNAPHEAPLIST = 0x00000001;
        private const uint TH32CS_SNAPPROCESS = 0x00000002;
        private const uint TH32CS_SNAPTHREAD = 0x00000004;
        private const uint TH32CS_SNAPMODULE = 0x00000008;
        private const uint TH32CS_SNAPMODULE32 = 0x00000010;
        private const uint TH32CS_SNAPALL = (TH32CS_SNAPHEAPLIST | TH32CS_SNAPPROCESS | TH32CS_SNAPTHREAD | TH32CS_SNAPMODULE);
        private const uint TH32CS_INHERIT = 0x80000000;

        private const int SE_PRIVILEGE_ENABLED = 0x00000002;
        private const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        private const int TOKEN_QUERY = 0x00000008;

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESSENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;          // this process
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;           // associated exe
            public uint cntThreads;
            public uint th32ParentProcessID;    // this process's parent process
            public uint pcPriClassBase;        // Base priority of process's threads
            public uint dwFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;   // Path
        }

        /// <summary>
        /// Open a process
        /// </summary>
        /// <param name="dwDesiredAccess">Access flag</param>
        /// <param name="bInheritHandle">Handle inheritance options</param>
        /// <param name="dwProcessId">Process identifier</param>
        /// <returns>Success</returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

        /// <summary>
        /// Terminate a (open) process
        /// </summary>
        /// <param name="dwProcessId">Handle</param>
        /// <param name="dwExitCode">Exit code</param>
        /// <returns>Success</returns>
        [DllImport("kernel32.dll")]
        private static extern Int32 TerminateProcess(UInt32 dwProcessId, UInt32 dwExitCode);

        /// <summary>
        /// Close a handle
        /// </summary>
        /// <param name="hObject">Handle to object</param>
        /// <returns>Success</returns>
        [DllImport("kernel32.dll")]
        private static extern Int32 CloseHandle(IntPtr hObject);

        /// <summary>
        /// Read from the memory of a process 
        /// </summary>
        /// <param name="hProcess">Handle to the process</param>
        /// <param name="lpBaseAddress">Base of memory area</param>
        /// <param name="buffer">Data buffer</param>
        /// <param name="size">Number of bytes to read</param>
        /// <param name="lpNumberOfBytesRead">Number of bytes read</param>
        /// <returns>Success</returns>
        [DllImport("kernel32.dll")]
        private static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr buffer, int size, ref IntPtr lpNumberOfBytesRead);

        /// <summary>
        /// Write to the memory of a process
        /// </summary>
        /// <param name="hProcess">Handle to the process</param>
        /// <param name="lpBaseAddress">Base of memory area</param>
        /// <param name="buffer">Data buffer</param>
        /// <param name="size">Number of bytes to read</param>
        /// <param name="lpNumberOfBytesWritten">Number of bytes read</param>
        /// <returns>Success</returns>
        [DllImport("kernel32.dll")]
        private static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr buffer, int size, ref IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll")]
        private static extern Int32 Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll")]
        private static extern Int32 Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            public long Luid;
            public int Attributes;
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int OpenProcessToken(int ProcessHandle, int DesiredAccess, ref int tokenhandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetCurrentProcess();

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LookupPrivilegeValue(string lpsystemname, string lpname, ref long lpLuid);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int AdjustTokenPrivileges(int tokenhandle, int disableprivs, ref TOKEN_PRIVILEGES Newstate, int bufferlength, int PreivousState, int Returnlength);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetSecurityInfo(int HANDLE, int SE_OBJECT_TYPE, int SECURITY_INFORMATION, int psidOwner, int psidGroup, out IntPtr pDACL, IntPtr pSACL, out IntPtr pSecurityDescriptor);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetSecurityInfo(int HANDLE, int SE_OBJECT_TYPE, int SECURITY_INFORMATION, int psidOwner, int psidGroup, IntPtr pDACL, IntPtr pSACL);

        private bool isOpen = false;
        private IntPtr eightBytes = IntPtr.Zero;
        private IntPtr hProcess = IntPtr.Zero;
        private Process readProcess = null;
        private EventHandler ExitedEvent = null;
        private Hashtable _parentids = new Hashtable();

        /// <summary>
        /// Returns the process handle of the open process
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return hProcess;
            }
        }

        /// <summary>
        /// A hashtable containing the process id's as key and the parent process id's as value
        /// </summary>
        public Hashtable ParentIds
        {
            get
            {
                return _parentids;
            }
        }

        /// <summary>
        /// Gets a list of processes by executable name
        /// </summary>
        /// <param name="ExeName">ExeName</param>
        /// <returns>List of processes</returns>
        public Process[] GetProcessesByExe(string ExeName)
        {
            ArrayList procs = new ArrayList();

            IntPtr hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
            if (hSnapshot == IntPtr.Zero)
                return null;

            PROCESSENTRY32 pe = new PROCESSENTRY32();
            pe.dwSize = 296;// sizeof( pe);

            // Clear the parent id's
            _parentids.Clear();

            int retval = Process32First(hSnapshot, ref pe);
            while (retval != 0)
            {
                if (pe.szExeFile.ToLower() == ExeName.ToLower())
                {
                    try
                    {
                        Process proc = Process.GetProcessById((int)pe.th32ProcessID);
                        procs.Add(proc);
                    }
                    catch
                    {
                    }
                }

                _parentids.Add(pe.th32ProcessID, pe.th32ParentProcessID);
                retval = Process32Next(hSnapshot, ref pe);
            }

            CloseHandle(hSnapshot);

            return (Process[])procs.ToArray(typeof(Process));
        }

        /// <summary>
        /// This closes a process using TerminateProcess (this is immediate!)
        /// </summary>
        public void CloseProcess()
        {
            TerminateProcess((uint)hProcess, 0);
            Close();
        }

        /// <summary>	
        /// Process from which to read		
        /// </summary>
        public Process ReadProcess
        {
            get
            {
                return readProcess;
            }
        }

        /// <summary>
        /// Is the current process opened
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
        }

        internal void EnableDebuggerPrivileges()
        {
            int token = 0;
            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES();
            tp.PrivilegeCount = 1;
            tp.Luid = 0;
            tp.Attributes = SE_PRIVILEGE_ENABLED;

            // We just assume this works
            if (OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref token) == 0)
                throw (new Exception("OpenProcessToken failed"));

            if (LookupPrivilegeValue(null, "SeDebugPrivilege", ref tp.Luid) == 0)
                throw (new Exception("LookupPrivilegeValue failed"));

            if (AdjustTokenPrivileges(token, 0, ref tp, Marshal.SizeOf(tp), 0, 0) == 0)
                throw (new Exception("AdjustTokenPrivileges failed"));
        }

        /// <summary>
        /// Open a process
        /// </summary>
        /// <remarks>
        /// Only use this for special occasions, normally use 'Start' from the WoW object
        /// </remarks>
        public void Open(Process process)
        {
            if (isOpen)
                throw (new Exception("Process already opened"));

            readProcess = process;

            hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ | PROCESS_TERMINATE, 0, (uint)readProcess.Id);
            if (hProcess == IntPtr.Zero)
            {
                IntPtr pDACL, pSecDesc;

                GetSecurityInfo((int)Process.GetCurrentProcess().Handle, /*SE_KERNEL_OBJECT*/ 6, /*DACL_SECURITY_INFORMATION*/ 4, 0, 0, out pDACL, IntPtr.Zero, out pSecDesc);
                hProcess = OpenProcess(0x40000, 0, (uint)process.Id);
                SetSecurityInfo((int)hProcess, /*SE_KERNEL_OBJECT*/ 6, /*DACL_SECURITY_INFORMATION*/ 4 | /*UNPROTECTED_DACL_SECURITY_INFORMATION*/ 0x20000000, 0, 0, pDACL, IntPtr.Zero);
                CloseHandle(hProcess);

                hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ | PROCESS_TERMINATE, 0, (uint)readProcess.Id);
            }

            isOpen = (hProcess != IntPtr.Zero);
            if (isOpen) readProcess.Exited += (ExitedEvent = new EventHandler(ProcessExited));
        }

        /// <summary>
        /// Close the process
        /// </summary>
        /// <remarks>
        /// Only use this for special occasions, normally use 'Stop' from the WoW object
        /// </remarks>
        public void Close()
        {
            if (hProcess == IntPtr.Zero)
                throw (new Exception("Process already closed"));

            int iRetValue;
            iRetValue = CloseHandle(hProcess);
            if (iRetValue == 0)
                throw new Exception("CloseHandle failed");

            hProcess = IntPtr.Zero;
            isOpen = false;

            readProcess.Exited -= ExitedEvent;
        }

        /// <summary>
        /// Initialize the memory reader class
        /// </summary>
        internal MemoryReader()
        {
            eightBytes = Marshal.AllocHGlobal(8);
        }

        /// <summary>
        /// Free the memory reader class
        /// </summary>
        ~MemoryReader()
        {
            if (isOpen)
                Close();

            Marshal.FreeHGlobal(eightBytes);
        }

        /// <summary>
        /// Read an integer from the currently opened process
        /// </summary>
        /// <param name="Address">Address to read from</param>
        /// <returns>Integer read</returns>
        public int ReadInteger(int Address)
        {
            IntPtr readedBytes = IntPtr.Zero;
            ReadProcessMemory(hProcess, new IntPtr(Address), eightBytes, 4, ref readedBytes);

            return Marshal.ReadInt32(eightBytes);
        }

        /// <summary>
        /// Read a long from the currently opened process
        /// </summary>
        /// <param name="Address">Address to read from</param>
        /// <returns>Long read</returns>
        public long ReadLong(int Address)
        {
            IntPtr readedBytes = IntPtr.Zero;
            ReadProcessMemory(hProcess, new IntPtr(Address), eightBytes, 8, ref readedBytes);

            return Marshal.ReadInt64(eightBytes);
        }

        /// <summary>
        /// Read a float from the currently opened process
        /// </summary>
        /// <param name="Address">Address to read from</param>
        /// <returns>Float read</returns>
        public float ReadFloat(int Address)
        {
            IntPtr readedBytes = IntPtr.Zero;
            ReadProcessMemory(hProcess, new IntPtr(Address), eightBytes, 4, ref readedBytes);
            byte[] buffer = new byte[4];
            Marshal.Copy(eightBytes, buffer, 0, 4);

            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Read a buffer from the currently opened process
        /// </summary>
        /// <param name="Address">Address to read from</param>
        /// <param name="bytes">Number of bytes to read</param>
        /// <returns>Buffer read</returns>
        public byte[] ReadBuffer(int Address, int bytes)
        {
            IntPtr ptr = Marshal.AllocHGlobal(bytes);
            IntPtr readedBytes = IntPtr.Zero;
            ReadProcessMemory(hProcess, new IntPtr(Address), ptr, bytes, ref readedBytes);
            byte[] ret = new byte[bytes];
            Marshal.Copy(ptr, ret, 0, bytes);
            Marshal.FreeHGlobal(ptr);

            return ret;
        }

        /// <summary>
        /// Read a [null-terminated] string from the currently opened process
        /// </summary>
        /// <param name="Address">Address to read from</param>
        /// <param name="bytes">Maximum size</param>
        /// <returns>String read</returns>
        public string ReadString(int Address, int bytes)
        {
            IntPtr ptr = Marshal.AllocHGlobal(bytes);
            IntPtr readedBytes = IntPtr.Zero;
            ReadProcessMemory(hProcess, new IntPtr(Address), ptr, bytes, ref readedBytes);
            byte[] buffer = new byte[bytes];
            Marshal.Copy(ptr, buffer, 0, bytes);
            Marshal.FreeHGlobal(ptr);

            UTF8Encoding utf8 = new UTF8Encoding();
            string result = utf8.GetString(buffer);
            int nullpos = result.IndexOf("\0");
            if (nullpos != -1)
                result = result.Remove(nullpos, result.Length - nullpos);
            return result;
        }

        /// <summary>
        /// Write an byte to the currently opened process
        /// </summary>
        /// <param name="Address">Address to write to</param>
        /// <param name="value">Value to write</param>
        public void WriteByte(long Address, byte value)
        {
            byte[] buffer = new byte[] { value };
            IntPtr writtenBytes = IntPtr.Zero;
            Marshal.Copy(buffer, 0, eightBytes, 1);
            WriteProcessMemory(hProcess, new IntPtr(Address), eightBytes, 1, ref writtenBytes);
        }

        /// <summary>
        /// Write an float to the currently opened process
        /// </summary>
        /// <param name="Address">Address to write to</param>
        /// <param name="value">Value to write</param>
        public void WriteFloat(int Address, float value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            IntPtr writtenBytes = IntPtr.Zero;
            Marshal.Copy(buffer, 0, eightBytes, 4);
            WriteProcessMemory(hProcess, new IntPtr(Address), eightBytes, 4, ref writtenBytes);
        }

        /// <summary>
        /// Write an integer to the currently opened process
        /// </summary>
        /// <param name="Address">Address to write to</param>
        /// <param name="value">Value to write</param>
        public void WriteInteger(int Address, int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            IntPtr writtenBytes = IntPtr.Zero;
            Marshal.Copy(buffer, 0, eightBytes, 4);
            WriteProcessMemory(hProcess, new IntPtr(Address), eightBytes, 4, ref writtenBytes);
        }

        /// <summary>
        /// Write a long to the currently opened process
        /// </summary>
        /// <param name="Address">Address to write to</param>
        /// <param name="value">Value to write</param>
        public void WriteLong(int Address, long value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            IntPtr writtenBytes = IntPtr.Zero;
            Marshal.Copy(buffer, 0, eightBytes, 8);
            WriteProcessMemory(hProcess, new IntPtr(Address), eightBytes, 8, ref writtenBytes);
        }

        /// <summary>
        /// Write a string to the currently opened process
        /// </summary>
        /// <param name="Address">Address to write to</param>
        /// <param name="value">Value to write</param>
        public void WriteString(int Address, string value)
        {
            byte[] buffer = System.Text.ASCIIEncoding.Default.GetBytes(value + '\0');
            IntPtr writtenBytes = IntPtr.Zero;
            IntPtr mBuffer = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, mBuffer, buffer.Length);
            WriteProcessMemory(hProcess, new IntPtr(Address), mBuffer, buffer.Length, ref writtenBytes);
            Marshal.FreeHGlobal(mBuffer);
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            if (sender == ReadProcess)
                isOpen = false;
        }
    }
}