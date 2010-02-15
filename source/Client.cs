using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Reanimator;

namespace Reanimator
{
    class Client
    {
        byte[] buffer;
        public byte[] Buffer
        {
            get
            {
                return buffer;
            }
        }

        public Client(byte[] byteArray)
        {
            buffer = byteArray;
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
            byte[] firstCheck = {   0x48, 0x83, 0xEC, 0x20,
                                    0xE8, 0x90, 0x90, 0x90, 0x90,
                                    0x85, 0xC0,
                                    0x75, 0x33,
                                    0xE8, 0x90, 0x90, 0x90, 0x90,
                                    0x48, 0x85, 0xC0 };
            int firstCheckIndex = FileTools.ByteArrayContains(buffer, firstCheck);
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
            int secondAndThirdChecksIndex = FileTools.ByteArrayContains(buffer, secondAndThirdChecks);
            if (secondAndThirdChecksIndex == -1)
            {
                return false;
            }

            buffer[firstCheckIndex + 12] -= 0x07;
            buffer[secondAndThirdChecksIndex + 22] += 0x01;
            buffer[secondAndThirdChecksIndex + 36] -= 0x01;

            return true;
        }
    }
}
