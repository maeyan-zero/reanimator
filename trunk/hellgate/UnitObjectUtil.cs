using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hellgate
{
    partial class UnitObject
    {
        [Serializable]
        public class UnitAppearance
        {
            [Serializable]
            public class EquippedItem
            {
                // if (bitTest(bitField1, 0x0F))                                
                // unknown;									            // 32                   Haven't encountered - saw in ASM though.

                public int ItemCode;									// 16
                public byte Unknown;

                // if (bitTest(bitField1, 0x00)) // if (bitField1 & 0x01)
                public int AffixCount;								    // 3
                public readonly List<int> AffixCodes;                   // 32 * affixCount

                public EquippedItem()
                {
                    AffixCodes = new List<int>();
                }
            };

            [Serializable]
            public class ModelWardrobeLayer
            {
                public UInt16 ItemCode;									// 16
                public bool UnknownBool;							    // 1
                public int UnknownBoolValue;					    	// 2
            };

            internal int EquippedItemCount;								// 3
            public readonly List<EquippedItem> EquippedItems;                                   // This only affects viewing from the select char screen

            public int Unknown1;										// 16

            // if (bitTest(bitField1, 0x16))
            public Int64 Unknown16;										// non-standard read in again

            // if (bitTest(bitField1, 0x23))
            public Int16 Unknown23;										// 16

            // if (bitTest(bitField1, 0x11))
            internal int WardrobeLayerHeadCount;						// 4
            public readonly List<int> WardrobeLayersHead;				// 16 * WardrobeLayerHeadCount

            internal int WardrobeAppearanceGroupCount;					// 3
            public readonly List<int> WardrobeAppearanceGroups;			// 16 * WardrobeAppearanceGroupCount

            internal int ColorCount;									// 4
            public readonly List<int> ColorPaletteIndicies;				// 8 * ColorCount       Order: Body, Hair, ??

            // if (testBit(pUnit->bitField1, 0x10))
            internal int WardrobeLayerCount;							// 16          This is the model appearance
            public readonly List<ModelWardrobeLayer> WardrobeLayers;

            public UnitAppearance()
            {
                EquippedItems = new List<EquippedItem>();
                WardrobeLayersHead = new List<int>();
                WardrobeAppearanceGroups = new List<int>();
                ColorPaletteIndicies = new List<int>();
                WardrobeLayers = new List<ModelWardrobeLayer>();
            }
        };

        [Serializable]
        public class StatBlock
        {
            public Stat GetStatByName(string name)
            {
                foreach (Stat stat in stats)
                {
                    if (stat.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return stat;
                    }
                }
                return null;
            }

            public Stat GetStatById(int id)
            {
                foreach (Stat stat in stats)
                {
                    if (stat.Id == id)
                    {
                        return stat;
                    }
                }
                return null;
            }

            [Serializable]
            public class Stat
            {
                [Serializable]
                public class Attribute
                {
                    public bool Exists;										    // 1 bit
                    public int BitCount;							            // 6 bits
                    public int Unknown1;							            // 2 bits
                    public bool Unknown11;							            // 1 bit		// if Unknown1 == 2
                    public bool HasTableCode;            					    // 1 bit		// if this is set, then don't read the table id below
                    public short TableCode;							            // 16 bits		// this is the excel table id to use
                };

                [Serializable]
                public class Values
                {
                    public int Attribute1;
                    public int Attribute2;
                    public int Attribute3;
                    public int StatValue; // I think...

                    public int AttributeAt(int index)
                    {
                        switch (index)
                        {
                            case 0: return Attribute1;
                            case 1: return Attribute2;
                            case 2: return Attribute3;
                        }

                        throw new IndexOutOfRangeException("Attribute ranges from 0-2.");
                    }
                }


                public Values GetAttributeByAttributeId(int id)
                {
                    foreach (Values value in values)
                    {
                        if (value.Attribute1 == id)
                        {
                            return value;
                        }
                    }

                    return null;
                }

                public Stat()
                {
                    Attributes = new List<Attribute>();
                }

                public int Row;                                                 // 11
                public short Code;											    // 16
                //public int attributesCount;							        // 2
                public List<Attribute> Attributes;                                          // tells the game if it's a skill id, or waypoint flag, etc
                public int BitCount;									        // 6		// size in bits of extra attribute
                public int OtherAttributeFlag;								    // 3		// i think that's what this is...		// can be 0x00, 0x01, 0x02, 0x03, 0x04
                public UnitStatOtherAttribute OtherAttribute;
                public int SkipResource;									    // 2		// resouce flag I think...
                public short Resource;										    // 16		// like in extra attributes - read if above is 0x00
                public bool RepeatFlag;										    // 1		// if set, check for repeat number
                public int RepeatCount;										    // 10		// i think this can be something other than 10 bits... I think...

                public List<Values> values;

                public short Id
                {
                    get { return Code; }
                    //if we need to add status entries...
                    set { Code = value; }
                }

                public Attribute Attribute1
                {
                    get { return AttributeAt(0); }
                }

                public Attribute Attribute2
                {
                    get { return AttributeAt(1); }
                }

                public Attribute Attribute3
                {
                    get { return AttributeAt(2); }
                }

                public Attribute AttributeAt(int index)
                {
                    return index >= Attributes.Count ? null : Attributes[index];
                }

                [XmlAttribute("Name")]
                public string Name { get; set; }

                public int Length
                {
                    get { return values.Count; }
                }

                public override string ToString()
                {
                    return Name + " : (0x" + Id.ToString("X") + " : " + Id + ")";
                }

                public Values this[int index]
                {
                    get { return values[index]; }
                }
            };

            public int Version;										        // 16 bits
            public int Usage;										        // 3 bits		// if 0, then we use as code, if 1, then we're using as row index, if 2... ?
            //public int additionalStatCount;							    // 6 bits
            public List<UnitStatAdditional> AdditionalStats;

            //public int statCount;										    // 16 bits
            public List<Stat> stats;

            public int NameCount;										    // 8 bits		// i think this has something to do with item affix/prefix naming
            public UnitStatName[] Names;

            public Stat this[int index]
            {
                get { return stats[index]; }
            }
        };

        [Serializable]
        public class UnitBitOffsets
        {
            public short Code;                                              // 16           // only seen as 0x3030 ('00' in ascii - assuming as "0th" index) - alert if otherwise
            public int Offset;                                              // 32           // only seen as offset to end of items bit offset
        }

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
        public class UnknownCount1FClass
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