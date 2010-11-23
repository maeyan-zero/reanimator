using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System;
using Revival.Common;

namespace Reanimator
{
    class Patches
    {
        public Hashtable AvailablePatches { get; private set; }
        MemoryReader memoryReader;

        string[] processList = 
        {
            "hellgate_sp_dx9_x86",
            "hellgate_sp_dx10_x86",
            "hellgate_sp_dx9_x64",
            "hellgate_sp_dx10_x64"
        };

        public Patches()
        {
            memoryReader = new MemoryReader();

            Hashtable HardcoreMode_dx9_x64 = new Hashtable();
            HardcoreMode_dx9_x64.Add((long)0x140205BA5, (byte)0x6E);
            HardcoreMode_dx9_x64.Add((long)0x1401A9838, (byte)0x75);
            HardcoreMode_dx9_x64.Add((long)0x1401A9846, (byte)0x74);

            Hashtable HardcoreMode = new Hashtable();
            HardcoreMode.Add("hellgate_sp_dx9_x64", HardcoreMode_dx9_x64);

            AvailablePatches = new Hashtable();
            AvailablePatches.Add("Hardcore Mode", HardcoreMode);
        }

        public Process OpenProcess()
        {
            Process[] processes = null;
            foreach (string application in processList)
            {
                processes = Process.GetProcessesByName(application);
                if (processes.Length == 1) break;
            }
            if (processes.Length != 1) return null;
            memoryReader.Open(processes[0]);
            memoryReader.EnableDebuggerPrivileges();
            return processes[0];
        }

        public bool ApplyPatches(string[] patches, string client)
        {
            foreach (string patch in patches)
            {
                Hashtable currentPatch = (Hashtable)AvailablePatches[patch];
                Hashtable patchVersion = (Hashtable)currentPatch[client];

                if (patchVersion == null) return false;

                foreach (DictionaryEntry instruction in patchVersion)
                {
                    long address = (long)instruction.Key;
                    byte value = (byte)instruction.Value;
                    memoryReader.WriteByte(address, value);
                }
            }

            return true;
        }

        public void CloseHandle()
        {
            memoryReader.Close();
        }

        public byte[] Buffer { get; private set; }

        public Patches(byte[] byteArray)
        {
            Buffer = byteArray;
        }

        public bool ApplyHardcorePatch()
        {
            // function HardcoreCheck
            /*
.text:0000000140205B9A                 sub     rsp, 20h							    // 48 83 EC 20
.text:0000000140205B9E                 call    sub_140002460						// E8 BD C8 DF FF               -> E8 * * * *
.text:0000000140205B9E
.text:0000000140205BA3                 test    eax, eax							    // 85 C0
.text:0000000140205BA5                 jnz     short loc_140205BDA					// 75 33
.text:0000000140205BA5
.text:0000000140205BA7                 call    sub_140164D8C						// E8 E0 F1 F5 FF               -> E8 * * * *
.text:0000000140205BA7
.text:0000000140205BAC                 test    rax, rax							    // 48 85 C0
             */

            // SPx86 (UnPacked.exe)
            /*
..rest:00486BC9 ; =============== S U B R O U T I N E =======================================
..rest:00486BC9
..rest:00486BC9
..rest:00486BC9 sub_486BC9      proc near               ; CODE XREF: sub_486E89+C3p
..rest:00486BC9                                         ; sub_48789B+5Fp ...
..rest:00486BC9                cmp     dword_A2D0AC, 1
..rest:00486BD0                 push    esi
..rest:00486BD1                 jz      short loc_486BFE
..rest:00486BD3                 call    sub_431844
..rest:00486BD8                 mov     esi, eax
..rest:00486BDA                 test    esi, esi
..rest:00486BDC                 jz      short loc_486BFE
..rest:00486BDE                 mov     eax, offset dword_B1EC78
..rest:00486BE3                 mov     ecx, esi
..rest:00486BE5                 call    sub_428033
..rest:00486BEA                 test    eax, eax
..rest:00486BEC                 jz      short loc_486BFE
..rest:00486BEE                 mov     eax, esi
..rest:00486BF0                 call    sub_42809F
..rest:00486BF5                 test    eax, eax
..rest:00486BF7                 jz      short loc_486BFE
..rest:00486BF9                 xor     eax, eax
..rest:00486BFB                 inc     eax
..rest:00486BFC                 pop     esi
..rest:00486BFD                 retn
             */

            byte[] firstCheck = {   0x48, 0x83, 0xEC, 0x20,
                                    0xE8, 0x90, 0x90, 0x90, 0x90,
                                    0x85, 0xC0,
                                    0x75, 0x33,
                                    0xE8, 0x90, 0x90, 0x90, 0x90,
                                    0x48, 0x85, 0xC0 };
            int firstCheckIndex = FileTools.ByteArrayContains(Buffer, firstCheck);
            if (firstCheckIndex == -1)
            {
                return false;
            }

            // function LoadChar_DoChecks
            /*
.text:00000001401A9822 cmp     eax, 1                                               // 83 F8 01
.text:00000001401A9825 jnz     short loc_1401A988C                                  // 75 65
.text:00000001401A9825
.text:00000001401A9827 lea     rdx, hardcoreBadge                                   // 48 8D 15 3A F8 83 00         -> 48 8D 15 * * * *
.text:00000001401A982E mov     rcx, r12                                             // 49 8B CC
.text:00000001401A9831 call    CheckHardcoreBadge                                   // E8 82 B5 16 00               -> E8 * * * *  
.text:00000001401A9831
.text:00000001401A9836 test    eax, eax                                             // 85 C0
.text:00000001401A9838 jz      short loc_1401A9848                                  // 74 0E
.text:00000001401A9838
.text:00000001401A983A xor     edx, edx                                             // 33 D2
.text:00000001401A983C mov     rcx, rbx                                             // 48 8B CB
.text:00000001401A983F call    sub_1401A8BA0                                        // E8 5C F3 FF FF               -> E8 * * * *
.text:00000001401A983F
.text:00000001401A9844 test    eax, eax                                             // 85 C0
.text:00000001401A9846 jnz     short loc_1401A988C                                  // 75 44
            */

            // SPx86
            /*
..rest:0043976E                 jnz     short loc_43979A
..rest:00439770                 mov     edx, [ebp+var_52C]
..rest:00439776                 push    offset dword_B1EC78
..rest:0043977B                 call    sub_566E51
..rest:00439780                 test    eax, eax
..rest:00439782                 pop     ecx
..rest:00439783                 jz      loc_4398E9
..rest:00439789                 push    edi
..rest:0043978A                 mov     eax, esi
..rest:0043978C                 call    sub_43FC6B
..rest:00439791                 test    eax, eax
..rest:00439793                 pop     ecx
..rest:00439794                 jz      loc_4398E
             */
            byte[] secondAndThirdChecks = {   0x83, 0xF8, 0x01,
                                              0x75, 0x65,
                                              0x48, 0x8D, 0x15, 0x90, 0x90, 0x90, 0x90,
                                              0x49, 0x8B, 0xCC,
                                              0xE8, 0x90, 0x90, 0x90, 0x90,
                                              0x85, 0xC0,
                                              0x74, 0x0E,
                                              0x33, 0xD2,
                                              0x48, 0x8B, 0xCB,
                                              0xE8, 0x90, 0x90, 0x90, 0x90,
                                              0x85, 0xC0,
                                              0x75, 0x44 };
            int secondAndThirdChecksIndex = FileTools.ByteArrayContains(Buffer, secondAndThirdChecks);
            if (secondAndThirdChecksIndex == -1)
            {
                return false;
            }

            Buffer[firstCheckIndex + 12] -= 0x07;
            Buffer[secondAndThirdChecksIndex + 22] += 0x01;
            Buffer[secondAndThirdChecksIndex + 36] -= 0x01;

            return true;
        }
    }
}
