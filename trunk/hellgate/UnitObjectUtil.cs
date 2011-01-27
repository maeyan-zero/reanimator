using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hellgate
{
    partial class UnitObject
    {
        // pretty sure this is populated based on a bit field... should check...
        [Serializable]
        public class UnitStatOtherAttribute
        {
            // if (otherAttributeFlag & 0x01)
            public int Unknown1;										// 4 bits
            // if (otherAttributeFlag & 0x02)
            public int Unknown2;										// 12 bits
            // if (otherAttributeFlag & 0x04)
            public int Unknown3;										// 1 bit		// possibly another reasource flag or something - if not 0x01 alert
        };

        [Serializable]
        public class UnitStatAdditional
        {
            public short Unknown;									    // 16 bits
            public short StatCount;										// 16 bits
            public List<StatBlock.Stat> Stats;
        };

        [Serializable]
        public class UnitStatName
        {
            public short Unknown1;										// 16 bits
            public StatBlock StatBlock;							        // nameCount * UnitStatBlock stuffs
        };

        [Serializable]
        public class UnknownCount1F
        {
            public short Unknown1;										// 16 bits
            public short Unknown2;										// 16 bits
        };

        [Serializable]
        public class UnitWeaponConfig
        {
            public short Id;                                            // 16 bits      // .text:00000001403DB5EA mov     edx, 4       .text:00000001403DB5EF call    ConvertNumber?  ; Call Pr
            public int UnknownCount1;                                   // 4 bits       // must be == 0x02
            public bool[] Exists1;                                      // 1 bit
            public int[] UnknownIds1;                                   // 32 bits
            public int UnknownCount2;                                   // 4 bits
            public bool[] Exists2;                                      // 1 bit
            public int[] UnknownIds2;                                   // 32 bits
            public int IdAnother;                                       // 32 bits
        };
    }
}
