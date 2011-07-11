using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Hellgate.Excel;
using Hellgate.Excel.JapaneseBeta;
using Hellgate.Excel.SinglePlayer;
using Revival.Common;

namespace Hellgate
{
    public class UnitObjectStats
    {
        public enum StatContext
        {
            UseCodes = 0,           // used when saving
            UseRows = 1,            // used when sending packets
            ExcelRowUseCodes = 2    // used in excel tables in Stat segments
        }

        private const int MaxParams = 4;
        private const int CurrentVersion = 10;

        [Serializable]
        public class StatSelector
        {
            public ushort Code;									    // 16               code from table STATS_SELECTOR  (if Context != 1)
            public int RowIndex;                                    // 5                row index from same table       (if Context == 1) // I think....
            //public short StatCount;                               // 16
            public readonly List<Stat> Stats = new List<Stat>();

            public ExcelTablesRow StatSelectorRow;

            public String Name { get { return (StatSelectorRow != null) ? StatSelectorRow.name : null; } }
        };

        [Serializable]
        public class StatName // this isn't a "StatName" - it's another StatSelector like above... not sure what difference is... leaving like this until determined
        {
            public short Unknown1;								    // 16 bits
            public UnitObjectStats Stats;						    // nameCount * UnitObjectStats stuffs
        };

        [Serializable]
        public class Stat
        {
            //[Serializable]
            //public class Param
            //{
            //    public bool Exists;                               // 1
            //    public int BitCount;                              // 6
            //    public int ParamOperationsFlags;                  // 2
            //    public int ParamShift;                            // 3
            //    public bool ParamIsOffset;                        // 1
            //    public bool NoTableCode;                          // 1		    if this is set, then don't read the table code below
            //    public UInt16 TableCode;                          // 16		    this is the excel table code to use
            //};

            [Serializable]
            public class StatValue
            {
                public int Param1;
                public Object Param1Row;
                public int Param2;
                public Object Param2Row;
                public int Param3;
                public Object Param3Row;
                public int Param4;
                public Object Param4Row;
                public int Value;
                public Object ValueRow;

                public void SetParamAt(int index, int value)
                {
                    if (index < 0 || index >= MaxParams) throw new IndexOutOfRangeException("if (index < 0 || index >= MaxParams (=4))");

                    switch (index)
                    {
                        case 0: Param1 = value; return;
                        case 1: Param2 = value; return;
                        case 2: Param3 = value; return;
                        case 3: Param4 = value; return;
                    }
                }

                public int GetParamAt(int index)
                {
                    if (index < 0 || index >= MaxParams) throw new IndexOutOfRangeException("if (index < 0 || index >= MaxParams (=4))");

                    switch (index)
                    {
                        case 0: return Param1;
                        case 1: return Param2;
                        case 2: return Param3;
                        case 3: return Param4;
                    }

                    return -1;
                }
            }

            // stat types = 0 = int, 1 = double?, 2 = float, 3+ = ?

            //public int RowIndex;                                  // 11
            //public UInt16 Code;                                   // 16
            //public int ParamCount;							    // 2            see Stats table - paramX columns
            //public List<Param> Params;
            //public int BitCount;                                  // 6		    bits to read in for stat value

            //public int ValueOperationsFlags;                      // 3		    operations upon the stat value (shift/offset/?)
            // if (OperationsFlags & 0x01)
            //public int ValueShift;                                // 4            valShift column in Stats
            // if (OperationsFlags & 0x02)
            //public int ValueOffset;                               // 12           valOffset column in Stats
            // if (OperationsFlags & 0x04)
            //public bool IsOffset;                                 // 1            offset column in Stats (unknown usage)

            //public int NoValueTableCode;                          // 2		    this is only tested for >= 3 (fail) and == 0 (get table code) - not sure what 1/2 do
            //public UInt16 ValueTableCode;                         // 16		    like in params - read if above is 0x00 - code value of ExcelTable

            //public bool HasMultipleValues;                        // 1	        if set, check for repeat number
            //public int ValueCount;                                // 7/10		    if Version <= 7, 7 bits, if version > 7, 10 bits

            public readonly List<StatValue> Values = new List<StatValue>();
            public StatsRow StatRow;

            public ExcelFile ValueTable { get { return (StatRow != null) ? StatRow.ValueExcelTable : null; } }
            public int ValueTableCount { get { return (StatRow != null) ? StatRow.ValueExcelTable.Rows.Count : 0; } }
            public ExcelFile Param1Table { get { return (StatRow != null) ? StatRow.Param1ExcelTable : null; } }
            public int Param1TableCount { get { return (StatRow != null) ? StatRow.Param2ExcelTable.Rows.Count : 0; } }
            public ExcelFile Param2Table { get { return (StatRow != null) ? StatRow.Param2ExcelTable : null; } }
            public int Param2TableCount { get { return (StatRow != null) ? StatRow.Param3ExcelTable.Rows.Count : 0; } }
            public ExcelFile Param3Table { get { return (StatRow != null) ? StatRow.Param3ExcelTable : null; } }
            public int Param3TableCount { get { return (StatRow != null) ? StatRow.Param4ExcelTable.Rows.Count : 0; } }
            public ExcelFile Param4Table { get { return (StatRow != null) ? StatRow.Param4ExcelTable : null; } }
            public int Param4TableCount { get { return (StatRow != null) ? StatRow.Param1ExcelTable.Rows.Count : 0; } }

            public Xls.TableCodes ValueTableCode { get { return (StatRow != null && StatRow.ValueExcelTable != null) ? StatRow.ValueExcelTable.TableCode : Xls.TableCodes.Null; } }
            public Xls.TableCodes Param1TableCode { get { return (StatRow != null && StatRow.Param1ExcelTable != null) ? StatRow.Param1ExcelTable.TableCode : Xls.TableCodes.Null; } }
            public Xls.TableCodes Param2TableCode { get { return (StatRow != null && StatRow.Param2ExcelTable != null) ? StatRow.Param2ExcelTable.TableCode : Xls.TableCodes.Null; } }
            public Xls.TableCodes Param3TableCode { get { return (StatRow != null && StatRow.Param3ExcelTable != null) ? StatRow.Param3ExcelTable.TableCode : Xls.TableCodes.Null; } }
            public Xls.TableCodes Param4TableCode { get { return (StatRow != null && StatRow.Param4ExcelTable != null) ? StatRow.Param4ExcelTable.TableCode : Xls.TableCodes.Null; } }

            public Xls.TableCodes GetParamAt(int index)
            {
                if (index < 0 || index >= MaxParams) throw new IndexOutOfRangeException("if (index < 0 || index >= MaxParams (=4))");

                switch (index)
                {
                    case 0: return Param1TableCode;
                    case 1: return Param2TableCode;
                    case 2: return Param3TableCode;
                    case 3: return Param4TableCode;
                }

                return Xls.TableCodes.Null;
            }

            public string Name { get { return (StatRow != null) ? StatRow.stat : null; } }
            public int Code { get { return (StatRow != null) ? StatRow.code : 0; } }
            public int Length { get { return Values.Count; } }

            public override string ToString()
            {
                return String.Format("{0} : {1} (0x{2:X4})", Name, Code, (uint)Code);
            }

            public StatValue this[int index]
            {
                get { return Values[index]; }
            }
        };

        private int _version;                                       // 16
        public StatContext Context;                                 // 3            0 = use code (character file), 1 = use row index (MP item drop or monster etc), 2 = use code (UnitData excel row)

        //public int StatSelectorCount;                             // 6
        public readonly List<StatSelector> StatSelectors;           // 21/32

        //public int StatCount;                                     // 16
        public readonly ConcurrentDictionary<Int32, Stat> Stats;

        public int NameCount;                                       // 8            i think this has something to do with item affix/prefix naming
        public readonly List<StatName> Names;

        ///////////////////// Function Definitions /////////////////////

        private readonly bool _debugOutputLoadingProgress;
        private readonly FileManager _fileManager;

        public UnitObjectStats(FileManager fileManager, bool debugOutputLoadingProgress, int version = CurrentVersion)
        {
            _fileManager = fileManager;
            _debugOutputLoadingProgress = debugOutputLoadingProgress;
            _version = version;

            StatSelectors = new List<StatSelector>();
            Stats = new ConcurrentDictionary<Int32, Stat>(2, 50);
            //Stats = new Dictionary<Xls.StatCodes, Stat>();
            Names = new List<StatName>();
        }

        public UnitObjectStats(FileManager fileManager, int version = CurrentVersion)
        {
            _fileManager = fileManager;
            _version = version;

            StatSelectors = new List<StatSelector>();
            Stats = new ConcurrentDictionary<Int32, Stat>(2, 50);
            //Stats = new Dictionary<Xls.StatCodes, Stat>();
            Names = new List<StatName>();
        }

        public Stat SetStat(String statName, int value, int? param1 = null, int? param2 = null, int? param3 = null, int? param4 = null)
        {
            return SetStatAt(_fileManager.GetStatRowFromName(statName), 0, value, param1, param2, param3, param4);
        }

        public Stat SetStat(Int16 statCode, int value, int? param1 = null, int? param2 = null, int? param3 = null, int? param4 = null)
        {
            return SetStatAt(_fileManager.GetStatRowFromCode(statCode), 0, value, param1, param2, param3, param4);
        }

        public Stat SetStatAt(String statName, int index, int value, int? param1 = null, int? param2 = null, int? param3 = null, int? param4 = null)
        {
            return SetStatAt(_fileManager.GetStatRowFromName(statName), index, value, param1, param2, param3, param4);
        }

        public Stat SetStatAt(Int16 statCode, int index, int value, int? param1 = null, int? param2 = null, int? param3 = null, int? param4 = null)
        {
            return SetStatAt(_fileManager.GetStatRowFromCode(statCode), index, value, param1, param2, param3, param4);
        }

        public Stat SetStatAt(StatsRow statData, int index, int value, int? param1 = null, int? param2 = null, int? param3 = null, int? param4 = null)
        {
            return Stats.AddOrUpdate(statData.code,
            (code) =>
             {
                int totalParams = 0;
                totalParams += (param1.HasValue ? 1 : 0);
                totalParams += (param2.HasValue ? 1 : 0);
                totalParams += (param3.HasValue ? 1 : 0);
                totalParams += (param4.HasValue ? 1 : 0);
                int[] paramValues = new int[totalParams];
                if (param1.HasValue) paramValues.SetValue(param1, 0);
                if (param2.HasValue) paramValues.SetValue(param2, 1);
                if (param3.HasValue) paramValues.SetValue(param3, 2);
                if (param4.HasValue) paramValues.SetValue(param4, 3);
                return _AddStat(statData, value, paramValues);
            },
            (code, stat) =>
            {
                if (stat.Values.Count > index) return null;

                lock (stat.Values)
                {
                    Stat.StatValue statValue;
                    if (stat.Values.Count == index)
                    {
                        statValue = new Stat.StatValue();
                        stat.Values.Add(statValue);
                    }
                    else
                    {
                        statValue = stat.Values[index];
                    }

                    statValue.Value = value;
                    if (param1.HasValue)
                    {
                        statValue.Param1 = param1.Value;
                        if ((statData.Param1ExcelTable != null && param1.Value >= 0 && param1.Value < statData.Param1ExcelTable.Rows.Count))
                        {
                            statValue.Param1Row = statData.Param1ExcelTable.Rows[param1.Value];
                        }
                    }
                    if (param2.HasValue)
                    {
                        statValue.Param2 = param2.Value;
                        if ((statData.Param2ExcelTable != null && param2.Value >= 0 && param2.Value < statData.Param2ExcelTable.Rows.Count))
                        {
                            statValue.Param2Row = statData.Param2ExcelTable.Rows[param2.Value];
                        }
                    }

                    if (param3.HasValue)
                    {
                        statValue.Param3 = param3.Value;
                        if ((statData.Param3ExcelTable != null && param3.Value >= 0 && param3.Value < statData.Param3ExcelTable.Rows.Count))
                        {
                            statValue.Param3Row = statData.Param3ExcelTable.Rows[param3.Value];
                        }
                    }

                    if (param4.HasValue)
                    {
                        statValue.Param4 = param4.Value;
                        if ((statData.Param4ExcelTable != null && param4.Value >= 0 && param4.Value < statData.Param4ExcelTable.Rows.Count))
                        {
                            statValue.Param4Row = statData.Param4ExcelTable.Rows[param4.Value];
                        }
                    }
                }

                return stat;
            }
            );
        }

        public Stat.StatValue GetStatValueAt(Int16 statCode, int index)
        {
            Stat stat = Stats.GetOrAdd(statCode, code => _AddStat(_fileManager.GetStatRowFromCode(statCode), 0, null));

            if (stat.Values.Count == 0) return null;

            lock (stat.Values)
            {
                if (stat.Values.Count > 0 && index < stat.Values.Count) return stat.Values[index];
            }

            return null;
        }

        public int GetStatValueOrDefault(Int16 statCode, int defaultValue = 0)
        {
            Stat.StatValue statValue = GetStatValueAt(statCode, 0);

            return (statValue == null) ? defaultValue : statValue.Value;
        }

        public int GetStatValueOrDefault(String stat, int defaultValue = 0)
        {
            int statCode = _fileManager.GetStatCodeFromName(stat);
            Stat.StatValue statValue = GetStatValueAt((short) statCode, 0);

            return (statValue == null) ? defaultValue : statValue.Value;
        }

        public List<Stat.StatValue> GetStatValues(Int32 statCode)
        {
            Stat stat;
            if (Stats.TryGetValue(statCode, out stat) && stat != null) return stat.Values;

            return null;
        }

        public int GetStatValue(String stat, int param1)
        {
            return GetStatValue(_fileManager.GetStatRowFromName(stat), param1);
        }

        public int GetStatValue(Int16 statCode, int param1)
        {
            return GetStatValue(_fileManager.GetStatRowFromCode(statCode), param1);
        }

        public int GetStatValue(StatsRow statsRow, int param1)
        {
            Stat stat;
            if (!Stats.TryGetValue(statsRow.code, out stat) || stat == null)
            {
                return (statsRow.ValueExcelTable == null) ? 0 : -1;
            }

            lock (stat.Values)
            {
                foreach (Stat.StatValue statValue in stat.Values)
                {
                    if (statValue.Param1 != param1) continue;

                    return statValue.Value;
                }
            }

            return (statsRow.ValueExcelTable == null) ? 0 : -1;
        }

        /// <summary>
        /// Adds or Updates a stat
        /// </summary>
        /// <param name="statsRow"></param>
        /// <param name="value"></param>
        /// <param name="paramValues"></param>
        /// <returns></returns>
        public Stat AddOrUpdate(StatsRow statsRow, int value, params int[] paramValues)
        {
            // debug check - ensure we're supplying the current/needed number of params... not sure if this is required or not yet
            Debug.Assert(statsRow.ParamCount == ((paramValues == null) ? 0 : paramValues.Length));

            //Stat.StatValue statValue;

            return Stats.AddOrUpdate(statsRow.code,
            (code) =>
            {
                //int defaultValue;
                //int[] defaultParamValues = new int[index];

                // We're assuming a default of 0 for values and -1 for tables - as of this writing, no param has a shift.
                // The paramXOffs is applicable only to table references i.e. it's akin to "offset" for stat value; not "valOffs"

                //if (index == 0)
                //{
                //    defaultValue = value;
                //}
                //else
                //{
                //    defaultValue = GetStatDefault(statsRow);
                //}

                //if (index == 1)
                //{
                //    defaultParamValues[0] = value;
                //}
                //else if (index > 1)
                //{
                //    defaultParamValues[0] = (statsRow.param1Table == -1) ? 0 : -1;
                //}

                //if (index == 2)
                //{
                //    defaultParamValues[1] = value;
                //}
                //else if (index > 2)
                //{
                //    defaultParamValues[1] = (statsRow.param2Table == -1) ? 0 : -1;
                //}

                //if (index == 3)
                //{
                //    defaultParamValues[2] = value;
                //}
                //else if (index > 3)
                //{
                //    defaultParamValues[2] = (statsRow.param3Table == -1) ? 0 : -1;
                //}

                //if (index == 4)
                //{
                //    defaultParamValues[3] = value;
                //}

                return _AddStat(statsRow, value, paramValues);
            },
            (code, stat) =>
            {
                lock (stat.Values)
                {
                    // do we already have a value?
                    Stat.StatValue currStatValue = null;
                    if (statsRow.ParamCount == 0 || paramValues == null)
                    {
                        Debug.Assert(stat.Values.Count <= 1); // if no params, then we can't have multiple values
                        if (stat.Values.Count == 1) currStatValue = stat.Values[0];
                    }
                    else
                    {
                        currStatValue = stat.Values.FirstOrDefault(statValue =>
                            (paramValues.Length == 1 && statValue.Param1 == paramValues[0]) ||
                            (paramValues.Length == 2 && statValue.Param1 == paramValues[0] && statValue.Param2 == paramValues[1]) ||
                            (paramValues.Length == 3 && statValue.Param1 == paramValues[0] && statValue.Param2 == paramValues[1] && statValue.Param3 == paramValues[2]) ||
                            (paramValues.Length == 3 && statValue.Param1 == paramValues[0] && statValue.Param2 == paramValues[1] && statValue.Param3 == paramValues[2] && statValue.Param4 == paramValues[3]));
                    }

                    if (currStatValue == null) // we need to add it
                    {
                        Stat.StatValue statValue = _CreateStatValue(statsRow, value, paramValues);
                        stat.Values.Add(statValue);
                    }
                    else if (currStatValue.Value != value)
                    {
                        currStatValue.Value = value;

                        Object valueRow = null;
                        if (statsRow.ValueExcelTable != null && value >= 0 && value < statsRow.ValueExcelTable.Rows.Count)
                        {
                            valueRow = statsRow.ValueExcelTable.Rows[value];
                        }
                        currStatValue.ValueRow = valueRow;
                    }
                }

                return stat;
            }
            );
        }

        public static int GetStatDefault(StatsRow statsRow)
        {
            if (statsRow.valTable != -1) return -1; // if it's a table stat, then return -1 for no row set

            // if a normal stat, then let's assuming 0 is default - we'll work in raw values, so apply offset(s)
            int retValue = statsRow.valOffs;
            if (statsRow.valOffs != 0 && statsRow.valShift != 0)
            {
                Debug.Assert(statsRow.valShift == 0); // not sure what to do if a stat has an offset and a shift... (shift then offset, or offset then shift?)
            }

            return retValue;
        }

        public Stat GetOrAddStat(StatsRow statsRow, int defaultValue, params int[] defaultParams)
        {
            return Stats.GetOrAdd(statsRow.code, code => _AddStat(statsRow, defaultValue, defaultParams));
        }

        private Stat _AddStat(StatsRow statsRow, int value, params int[] paramValues)
        {
            Stat stat;
            lock (Stats)
            {
                if (Stats.TryGetValue(statsRow.code, out stat) && stat != null) return stat;

                stat = new Stat { StatRow = statsRow };

                //Object valueRow = null;
                //if (statsRow.ValueExcelTable != null && value >= 0 && value < statsRow.ValueExcelTable.Rows.Count)
                //{
                //    valueRow = statsRow.ValueExcelTable.Rows[value];
                //}

                //Stat.StatValue statValue = new Stat.StatValue { Value = value, ValueRow = valueRow };
                //stat.Values.Add(statValue);

                Stat.StatValue statValue = _CreateStatValue(statsRow, value, paramValues);
                stat.Values.Add(statValue);
            }

            return stat;
        }

        private static Stat.StatValue _CreateStatValue(StatsRow statsRow, int value, int[] paramValues)
        {
            Object valueRow = null;
            if (statsRow.ValueExcelTable != null && value >= 0 && value < statsRow.ValueExcelTable.Rows.Count)
            {
                valueRow = statsRow.ValueExcelTable.Rows[value];
            }

            Stat.StatValue statValue = new Stat.StatValue { Value = value, ValueRow = valueRow };
            if (paramValues == null || paramValues.Length == 0) return statValue;

            if (paramValues.Length >= 1)
            {
                statValue.Param1 = paramValues[0];
                if ((statsRow.Param1ExcelTable != null && paramValues[0] >= 0 && paramValues[0] < statsRow.Param1ExcelTable.Rows.Count))
                {
                    statValue.Param1Row = statsRow.Param1ExcelTable.Rows[paramValues[0]];
                }
            }
            if (paramValues.Length >= 2)
            {
                statValue.Param2 = paramValues[1];
                if ((statsRow.Param2ExcelTable != null && paramValues[1] >= 0 && paramValues[1] < statsRow.Param2ExcelTable.Rows.Count))
                {
                    statValue.Param2Row = statsRow.Param2ExcelTable.Rows[paramValues[1]];
                }
            }
            if (paramValues.Length >= 3)
            {
                statValue.Param3 = paramValues[2];
                if ((statsRow.Param3ExcelTable != null && paramValues[2] >= 0 && paramValues[2] < statsRow.Param3ExcelTable.Rows.Count))
                {
                    statValue.Param3Row = statsRow.Param3ExcelTable.Rows[paramValues[2]];
                }
            }
            if (paramValues.Length >= 4)
            {
                statValue.Param4 = paramValues[3];
                if ((statsRow.Param4ExcelTable != null && paramValues[3] >= 0 && paramValues[3] < statsRow.Param4ExcelTable.Rows.Count))
                {
                    statValue.Param4Row = statsRow.Param4ExcelTable.Rows[paramValues[3]];
                }
            }

            return statValue;
        }

        /// <summary>
        /// Read stats from a serialised byte array
        /// </summary>
        /// <param name="bitBuffer">The current buffer state.</param>
        /// <param name="readNameCount">TODO</param>
        public void ReadStats(BitBuffer bitBuffer, bool readNameCount = false)
        {
            ReadStats(bitBuffer, this, readNameCount);
        }

        /// <summary>
        /// Read stats from a serialised byte array
        /// </summary>
        /// <param name="bitBuffer">The current buffer state.</param>
        /// <param name="stats">The stats block to populate.</param>
        /// <param name="readNameCount">TODO</param>
        public void ReadStats(BitBuffer bitBuffer, UnitObjectStats stats, bool readNameCount = false)
        {
            stats._version = bitBuffer.ReadBits(16);
            stats.Context = (StatContext)bitBuffer.ReadBits(3);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("stats.Version = {0}, stats.Context = {1} (0x{2:X2})", stats._version, stats.Context, (int)stats.Context));
            }
            if (stats._version != 0x0A)
            {
                throw new Exceptions.NotSupportedVersionException("0x0A", "0x" + stats._version.ToString("X2"));
            }

            int statSelectorCount = bitBuffer.ReadBits(6);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("stats.StatSelectorCount = {0}", statSelectorCount));
            }
            for (int i = 0; i < statSelectorCount; i++)
            {
                StatSelector statSelector = new StatSelector();

                if (stats.Context == StatContext.UseRows)
                {
                    statSelector.RowIndex = bitBuffer.ReadBits(5);
                    statSelector.StatSelectorRow = (ExcelTablesRow)_fileManager.GetRowFromIndex(Xls.TableCodes.STATS_SELECTOR, statSelector.RowIndex); // StatsSelector table
                }
                else
                {
                    statSelector.Code = bitBuffer.ReadUInt16();
                    statSelector.StatSelectorRow = (ExcelTablesRow)_fileManager.GetRowFromCode(Xls.TableCodes.STATS_SELECTOR, (short)statSelector.Code); // StatsSelector table
                }

                int selectorStatCount = bitBuffer.ReadInt16();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("StatSelectors[{0}].Name = {1}, .Code = 0x{2:X4}, .RowIndex = {3}, .StatCount = {4}",
                        i, statSelector.Name, statSelector.Code, statSelector.RowIndex, selectorStatCount));
                }

                for (int j = 0; j < selectorStatCount; j++)
                {
                    Stat stat = _ReadStat(bitBuffer, stats);
                    statSelector.Stats.Add(stat);
                }

                stats.StatSelectors.Add(statSelector);
            }

            int statCount = bitBuffer.ReadBits(16);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("stats.StatCount = {0}", statCount));
            }
            for (int i = 0; i < statCount; i++)
            {
                Stat stat = _ReadStat(bitBuffer, stats);
                stats.Stats[stat.StatRow.code] = stat;
            }


            if (!readNameCount) return;
            stats.NameCount = bitBuffer.ReadBits(8);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("stats.NameCount = {0}", stats.NameCount));
            }

            for (int i = 0; i < stats.NameCount; i++)
            {
                StatName name = new StatName
                {
                    Unknown1 = (short)bitBuffer.ReadBits(5),
                    Stats = new UnitObjectStats(_fileManager, _version)
                };
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("stats.Unknown1 = {0}", name.Unknown1));
                }

                ReadStats(bitBuffer, name.Stats, false);
                stats.Names.Add(name);
            }
        }

        private Stat _ReadStat(BitBuffer bitBuffer, UnitObjectStats statBlock)
        {
            Stat stat = new Stat();

            int valueShift = 0;
            int valueOffset = 0;
            bool isOffset = false;
            StatsRow statData;

            int valueBitCount;
            int[] paramsBitCounts = new int[MaxParams];

            if (statBlock.Context == StatContext.UseRows)
            {
                int rowIndex = bitBuffer.ReadBits(11); // todo: make this a static value - this bit count is determined by FileManager._GetTableRowBitMax() of the Stats row count
                statData = stat.StatRow = _fileManager.GetStatRowFromIndex(rowIndex);
                if (statData == null) throw new Exceptions.UnitObjectException(String.Format("Error stat.RowIndex = {0} not found.\nCannot have null stat - not known param count will break bit offset.", rowIndex));
                valueBitCount = statData.valbits;

                if (statData.ParamCount > 0 && bitBuffer.ReadBool()) paramsBitCounts[0] = statData.param1Bits;
                if (statData.ParamCount > 1 && bitBuffer.ReadBool()) paramsBitCounts[1] = statData.param2Bits;
                if (statData.ParamCount > 2 && bitBuffer.ReadBool()) paramsBitCounts[2] = statData.param3Bits;
                if (statData.ParamCount > 3 && bitBuffer.ReadBool()) paramsBitCounts[3] = statData.param4Bits;

                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("stat.Name = {0}, .Code = {1} (0x{2:X4}), .ParamCount = {3}", stat.Name, stat.Code, (uint)stat.Code, statData.ParamCount));

                    Xls.TableCodes tableCode = (stat.ValueTable == null) ? Xls.TableCodes.Null : stat.ValueTable.TableCode;
                    int noValueTableCode = (stat.ValueTable == null) ? 1 : 0;
                    Debug.WriteLine(String.Format("stat.NoValueTableCode = {2}, .ValueTableCode = 0x{0:X4}, .ValueTable = {1}", (uint)tableCode, tableCode, noValueTableCode));

                    if (statData.ParamCount >= 1) Debug.WriteLine(String.Format("param1.BitCount = {0}, .Table = {1}", paramsBitCounts[0], stat.Param1TableCode));
                    if (statData.ParamCount >= 2) Debug.WriteLine(String.Format("param2.BitCount = {0}, .Table = {1}", paramsBitCounts[1], stat.Param2TableCode));
                    if (statData.ParamCount >= 3) Debug.WriteLine(String.Format("param3.BitCount = {0}, .Table = {1}", paramsBitCounts[2], stat.Param3TableCode));
                    if (statData.ParamCount >= 4) Debug.WriteLine(String.Format("param4.BitCount = {0}, .Table = {1}", paramsBitCounts[3], stat.Param4TableCode));
                }
            }
            else
            {
                ushort code = bitBuffer.ReadUInt16();
                statData = stat.StatRow = _fileManager.GetStatRowFromCode((short)code);
                if (stat.StatRow == null) throw new Exceptions.UnitObjectException(String.Format("Error: stat.Code = {0} (0x{0:X4}) not found.\nCannot have null stat - not known param count will break bit offset.", code));

                int paramsCount = bitBuffer.ReadBits(2);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("stat.Name = {0}, stat.Code = {1} (0x{2:X4}), stat.ParamCount = {3}", stat.Name, code, (uint)code, paramsCount));
                }
                if (paramsCount > statData.ParamCount) Debug.WriteLine(String.Format("Unexpected large params count of {0}. Expecting count <= {1}", paramsCount, statData.ParamCount));

                for (int i = 0; i < paramsCount; i++)
                {
                    bool exists = bitBuffer.ReadBool();
                    if (!exists) continue;

                    paramsBitCounts[i] = bitBuffer.ReadBits(6);

                    int paramOperationsFlags = bitBuffer.ReadBits(2);
                    int paramShift = 0;
                    bool paramIsOffset = false;
                    if ((paramOperationsFlags & 0x01) != 0)
                    {
                        paramShift = bitBuffer.ReadBits(3);
                    }
                    if ((paramOperationsFlags & 0x02) != 0)
                    {
                        paramIsOffset = bitBuffer.ReadBool();
                    }

                    bool hasTableCode = !bitBuffer.ReadBool();
                    Xls.TableCodes paramTableCode = Xls.TableCodes.Null;
                    if (hasTableCode)
                    {
                        paramTableCode = (Xls.TableCodes)bitBuffer.ReadUInt16();

                        Xls.TableCodes expetedTableCode = Xls.TableCodes.Null;
                        switch (i)
                        {
                            case 0: expetedTableCode = stat.Param1TableCode; break;
                            case 1: expetedTableCode = stat.Param2TableCode; break;
                            case 2: expetedTableCode = stat.Param3TableCode; break;
                            case 3: expetedTableCode = stat.Param4TableCode; break;
                        }

                        if (expetedTableCode != paramTableCode) throw new Exceptions.UnitObjectException(String.Format("Unexpected param value table supplied. Expecting {0}, got {1}", expetedTableCode, paramTableCode));
                    }

                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("param.BitCount = {0}, .ParamOperationsFlags = {1}, .ParamShift = {2}, .ParamIsOffset = {3}, .HasTableCode = {4}, .TableCode = {5}, .Table = {6}",
                            paramsBitCounts[i], paramOperationsFlags, paramShift, paramIsOffset, hasTableCode, (uint)paramTableCode, paramTableCode));
                    }
                }

                valueBitCount = bitBuffer.ReadBits(6);

                int valueOperationsFlags = bitBuffer.ReadBits(3);
                if ((valueOperationsFlags & 0x01) != 0)
                {
                    valueShift = bitBuffer.ReadBits(4); // valShift
                }
                if ((valueOperationsFlags & 0x02) != 0)
                {
                    valueOffset = bitBuffer.ReadBits(12); // valOffset
                }
                if ((valueOperationsFlags & 0x04) != 0)
                {
                    isOffset = bitBuffer.ReadBool();
                }
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("stat.BitCount = {0}, .ValueOperationsFlags = {1}, .ValueShift = {2}, .ValueOffset = {3}, .IsOffset = {4}",
                        valueBitCount, valueOperationsFlags, valueShift, valueOffset, isOffset));
                }

                int noValueTableCode = bitBuffer.ReadBits(2);
                //Debug.Assert(noValueTableCode != 2); // never seen... seen in stats column in Monsters table

                Xls.TableCodes tableCode = Xls.TableCodes.Null;
                if (noValueTableCode == 0)
                {
                    tableCode = (Xls.TableCodes)bitBuffer.ReadUInt16();

                    Xls.TableCodes expectedTableCode = stat.ValueTableCode;
                    if (expectedTableCode != tableCode) throw new Exceptions.UnitObjectException(String.Format("Unexpected stat value table supplied. Expecting {0}, got {1}", expectedTableCode, tableCode));
                }
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("stat.NoValueTableCode = {0}, .ValueTableCode = 0x{1:X4}, .ValueTable = {2}", noValueTableCode, (uint)tableCode, tableCode));
                }
            }

            bool hasMultipleValues = bitBuffer.ReadBool();
            int valueCount = 1;
            if (hasMultipleValues)
            {
                int valueCountBits = (statBlock._version <= 7) ? 7 : 10;
                valueCount = bitBuffer.ReadBits(valueCountBits);
            }

            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("stat.HasMultipleValues = {0}, .ValueCount = {1}", hasMultipleValues, valueCount));
            }

            for (int i = 0; i < valueCount; i++)
            {
                Stat.StatValue statValue = new Stat.StatValue();

                _ReadStatValue(bitBuffer, paramsBitCounts[0], ref statValue.Param1, stat.Param1Table, ref statValue.Param1Row, statBlock.Context);
                _ReadStatValue(bitBuffer, paramsBitCounts[1], ref statValue.Param2, stat.Param2Table, ref statValue.Param2Row, statBlock.Context);
                _ReadStatValue(bitBuffer, paramsBitCounts[2], ref statValue.Param3, stat.Param3Table, ref statValue.Param3Row, statBlock.Context);
                _ReadStatValue(bitBuffer, paramsBitCounts[3], ref statValue.Param4, stat.Param4Table, ref statValue.Param4Row, statBlock.Context);

                _ReadStatValue(bitBuffer, valueBitCount, ref statValue.Value, stat.ValueTable, ref statValue.ValueRow, statBlock.Context);

                // this seems to work
                // todo: do this for writing stats
                if (valueOffset == 0)
                    valueOffset = (stat.StatRow.valOffs + stat.StatRow.offset);
                if (valueShift == 0)
                    valueShift = (stat.StatRow.valShift - stat.StatRow.shift);

                statValue.Value -= valueOffset; // not going to bother with this for now
                statValue.Value <<= valueShift; // not sure what this is for either - possibly server-side to increase accuracy/value range I guess - but values aren't saved with it...
                //if (value.IsOffset) statValue.Value--; // do this to *row index* though, not code - not sure about non-table relations values...

                stat.Values.Add(statValue);

                if (_debugOutputLoadingProgress)
                {
                    if (paramsBitCounts[0] > 0) Debug.WriteLine("stat.Param1 = " + statValue.Param1);
                    if (paramsBitCounts[1] > 0) Debug.WriteLine("stat.Param2 = " + statValue.Param2);
                    if (paramsBitCounts[2] > 0) Debug.WriteLine("stat.Param3 = " + statValue.Param3);
                    if (paramsBitCounts[3] > 0) Debug.WriteLine("stat.Param4 = " + statValue.Param4);
                    Debug.WriteLine(String.Format("stat.Value = {0}", statValue.Value));
                }
            }

            return stat;
        }

        private void _ReadStatValue(BitBuffer bitBuffer, int bitCount, ref int value, ExcelFile valueTable, ref Object valueRow, StatContext statContext)
        {
            if (bitCount <= 0) return;

            value = bitBuffer.ReadBits(bitCount);
            if (valueTable == null) return;

            if (statContext == StatContext.UseRows)
            {
                if (value >= 0 && value < valueTable.Rows.Count)
                {
                    valueRow = valueTable.Rows[value];
                }
                if (valueRow == null) Debug.WriteLine(String.Format("Warning: OutOfBounds stat index {0} on table {1}", value, valueTable.TableCode));
            }
            else
            {
                valueRow = _fileManager.GetRowFromCode(valueTable.TableCode, (short)value);
                if (valueRow == null) Debug.WriteLine(String.Format("Warning: Unknown stat value code {0} (0x{0:X4}) on table {1}", value, valueTable.TableCode));
            }
        }

        public byte[] WriteStats(bool writeNameCount = false)
        {
            BitBuffer bitBuffer = new BitBuffer();
            bitBuffer.CreateBuffer();
            WriteStats(bitBuffer, this, writeNameCount);
            return bitBuffer.GetBuffer();
        }

        public void WriteStats(BitBuffer bitBuffer, bool writeNameCount = false)
        {
            WriteStats(bitBuffer, this, writeNameCount);
        }

        public void WriteStats(BitBuffer bitBuffer, UnitObjectStats stats, bool writeNameCount = false)
        {
            /***** Stat Block Header *****
             * Version                                          16                  Stat block header - Must be 0x000A.
             * Context                                          3                   Use 0 for Code usage, 1 for RowIndex usage, 2 for... ?
             * 
             * StatSelectorCount                                6                   Not sure of use - but uses StatSelector table
             * {
             *      Code                                        16                  Code from StatSelector table     (if Context != 1)
             *      RowIndex                                    5                   RowIndex from StatSelector table (if Context == 1)
             *      
             *      StatCount                                   16                  Count of following stats.
             *      {
             *          STATS                                                       See WriteStat().
             *      }
             * }
             * 
             * StatCount                                        16                  Count of following stats.
             * {
             *      STATS                                                           See WriteStat().
             * }
             * 
             * // todo: update me (same StatSelector stuff from above)
             * if (WriteNameCount)                                                  This is TRUE by default. Set to FALSE when writing a stat block
             * {                                                                    from the below name stat block chunk.
             *      NameCount                                   8                   I think this has something to do with item names.
             *      {
             *          Code                                    16                  Code from StatSelector table     (if Context != 1)
             *          RowIndex                                5                   RowIndex from StatSelector table (if Context == 1)
             *          
             *          STAT BLOCK                                                  See WriteStatBlock().
             *      }
             * }
             */

            bitBuffer.WriteBits(stats._version, 16);
            bitBuffer.WriteBits((int)stats.Context, 3);

            bitBuffer.WriteBits(stats.StatSelectors.Count, 6);
            foreach (StatSelector statSelector in stats.StatSelectors)
            {
                if (stats.Context == StatContext.UseRows)
                {
                    bitBuffer.WriteBits(statSelector.RowIndex, 5);
                }
                else
                {
                    bitBuffer.WriteUInt16(statSelector.Code);
                }

                bitBuffer.WriteBits(statSelector.Stats.Count, 16);
                foreach (Stat stat in statSelector.Stats)
                {
                    _WriteStat(bitBuffer, stats, stat);
                }
            }

            bitBuffer.WriteBits(stats.Stats.Count, 16);

            foreach (Stat stat in stats.Stats.Values.OrderBy(stat => stat.Code))
            {
                _WriteStat(bitBuffer, stats, stat);
            }

            if (!writeNameCount) return;

            bitBuffer.WriteBits(stats.Names.Count, 8);
            foreach (StatName statName in stats.Names)
            {
                bitBuffer.WriteBits(statName.Unknown1, 16);
                WriteStats(bitBuffer, statName.Stats, false);
            }
        }

        private void _WriteStat(BitBuffer bitBuffer, UnitObjectStats statBlock, Stat stat)
        {
            /***** Stat Block *****
             * if (Context == 1)
             * {
             *      RowIndex                                    11                  RowIndex to Stats table - used in MP context.
             * }
             * else
             * {
             *      Code                                        16                  Code from Stats excel table.
             * 
             *      ParamsCount                                 2                   Count of following.
             *      {
             *          Exists                                  1                   Simple bool test.
             *          {
             *              ParamBitCount                       6                   Number of bits used in file.
             *          
             *              ParamOperationsFlags                2                   Operations to be done to param value
             *              if (ParamOperationsFlags & 0x01)
             *              {
             *                  ParamShift                      3                   Not seen used - but is in ASM.
             *              }
             *              if (ParamOperationsFlags & 0x02)
             *              {
             *                  ParamIsOffset                   1                   Param is offset - unknown usage.
             *              }
             *          
             *              NoTableCode                         1                   Bool type.
             *              if (!NoTableCode)
             *              {
             *                  ParamTableCode                  16                  Like StatCode, allows param value to refer to an excel table.
             *              }
             *          }
             *      }
             * 
             *      BitCount                                    6                   Number of bits used in file for stat value.
             * 
             *      ValueOperationsFlags                        3                   Operations to be performed on stat value.
             *      if (otherAttributeFlag & 0x01)
             *      {
             *          ValueShift                              4                   Value shift - unknown usage. Probably used server-side to allow for greater ranges.
             *      }
             *      if (otherAttributeFlag & 0x02)
             *      {
             *          ValueOffset                             12                  Value if offset. Real value: Value -= ValueOffset;
             *      }
             *      if (otherAttributeFlag & 0x04)
             *      {
             *          ValueIsOffset                           1                   Not to be confused with ValueOffset - IsOffset usage unknown (similar to ParamIsOffset)
             *      }
             * 
             *      NoValueTableCode                            2                   ASM only considered 0x00 - any other value and ValueTableCode is not read in.
             *      if (!NoValueTableCode)
             *      {
             *          ValueTableCode                          16                  Allows value to refer to an excel table.
             *      }
             *      
             * } // end if (Context == 1)
             * 
             * 
             * HasMultipleValues                                1                   Bool type.
             * {
             *      ValueCount                                  10                  Number of times to read in stat values.
             * }
             * 
             * for (ValueCount)                                                     If HasMultipleValues == false, then obviously we still want to read
             * {                                                                    in at least once... So really it's like a do {} while() chunk.
             *      for (ParamsCount)
             *      {
             *          ParamValue                              ParamBitCount       The extra attribute for the applicable value below.
             *      }
             *      
             *      StatValue                                   BitCount            The actual stat value.
             * }
             */

            StatsRow statData = stat.StatRow;
            int valueBitCount;
            int[] paramsBitCounts = new int[MaxParams];

            if (statBlock.Context == StatContext.UseRows)
            {
                valueBitCount = statData.valbits;
                paramsBitCounts[0] = statData.param1Bits; // todo: this could be an issue if we have to write a value greater than the bits can handle
                paramsBitCounts[1] = statData.param2Bits;
                paramsBitCounts[2] = statData.param3Bits;
                paramsBitCounts[3] = statData.param4Bits;

                int rowIndex = _fileManager.GetStatRowIndexFromCode((short) statData.code);
                if (rowIndex == -1) throw new Exceptions.UnitObjectException("Error: Stat row index not found for Stat = " + statData);
                bitBuffer.WriteBits(rowIndex, 11);

                if (statData.ParamCount > 0) bitBuffer.WriteBool(true);
                if (statData.ParamCount > 1) bitBuffer.WriteBool(true);
                if (statData.ParamCount > 2) bitBuffer.WriteBool(true);
                if (statData.ParamCount > 3) bitBuffer.WriteBool(true);
            }
            else
            {
                valueBitCount = (statData.valTable == -1) ? statData.valbits : 32;  // on sp side - the bit count is equal to the bit count of the code field (32 for int, 16 for short, etc), but this will do
                paramsBitCounts[0] = (statData.param1Table == -1) ? statData.param1Bits : 32; // todo: this could be an issue if we have to write a value greater than the bits can handle
                paramsBitCounts[1] = (statData.param2Table == -1) ? statData.param2Bits : 32;
                paramsBitCounts[2] = (statData.param3Table == -1) ? statData.param3Bits : 32;
                paramsBitCounts[3] = (statData.param4Table == -1) ? statData.param4Bits : 32;

                bitBuffer.WriteUInt16((uint)statData.code);
                bitBuffer.WriteBits(statData.ParamCount, 2);

                if (statData.ParamCount > 0) _WriteStatParamData(bitBuffer, paramsBitCounts[0], statData.param1Shift, statData.param1Offs != 0, stat.Param1TableCode);
                if (statData.ParamCount > 1) _WriteStatParamData(bitBuffer, paramsBitCounts[1], statData.param2Shift, statData.param2Offs != 0, stat.Param2TableCode);
                if (statData.ParamCount > 2) _WriteStatParamData(bitBuffer, paramsBitCounts[2], statData.param3Shift, statData.param3Offs != 0, stat.Param3TableCode);
                if (statData.ParamCount > 3) _WriteStatParamData(bitBuffer, paramsBitCounts[3], statData.param4Shift, statData.param4Offs != 0, stat.Param4TableCode);

                bitBuffer.WriteBits(valueBitCount, 6);

                int valueOperationsFlags = 0;
                if (stat.StatRow.valShift > 0) valueOperationsFlags |= 0x01;
                if (stat.StatRow.valOffs > 0) valueOperationsFlags |= 0x02;
                if (stat.StatRow.offset > 0) valueOperationsFlags |= 0x04;

                bitBuffer.WriteBits(valueOperationsFlags, 3);
                if ((valueOperationsFlags & 0x01) != 0)
                {
                    bitBuffer.WriteBits(stat.StatRow.valShift, 4);
                }
                if ((valueOperationsFlags & 0x02) != 0)
                {
                    bitBuffer.WriteBits(stat.StatRow.valOffs, 12);
                }
                if ((valueOperationsFlags & 0x04) != 0)
                {
                    bitBuffer.WriteBool(true);
                }

                int noValueTableCode = (stat.ValueTable != null) ? 0 : 1;
                bitBuffer.WriteBits(noValueTableCode, 2);
                if (stat.ValueTable != null)
                {
                    bitBuffer.WriteUInt16((uint)stat.ValueTableCode);
                }
            }

            int valueCount = stat.Values.Count;
            bool hasMultipleValues = (valueCount > 1);

            bitBuffer.WriteBool(hasMultipleValues);
            if (hasMultipleValues)
            {
                int valueCountBits = (statBlock._version <= 7) ? 7 : 10;
                bitBuffer.WriteBits(valueCount, valueCountBits);
            }

            foreach (Stat.StatValue statValue in stat.Values)
            {
                if (statData.ParamCount >= 1) _WriteStatValue(bitBuffer, statValue.Param1, statValue.Param1Row, stat.Param1Table, paramsBitCounts[0], statBlock.Context);
                if (statData.ParamCount >= 2) _WriteStatValue(bitBuffer, statValue.Param2, statValue.Param2Row, stat.Param2Table, paramsBitCounts[1], statBlock.Context);
                if (statData.ParamCount >= 3) _WriteStatValue(bitBuffer, statValue.Param3, statValue.Param3Row, stat.Param3Table, paramsBitCounts[2], statBlock.Context);
                if (statData.ParamCount >= 4) _WriteStatValue(bitBuffer, statValue.Param4, statValue.Param4Row, stat.Param4Table, paramsBitCounts[3], statBlock.Context);

                int valueOffset = (stat.StatRow.valOffs + stat.StatRow.offset);
                int valueShift = (stat.StatRow.valShift - stat.StatRow.shift);

                statValue.Value += valueOffset;
                statValue.Value >>= valueShift;

                _WriteStatValue(bitBuffer, statValue.Value, statValue.ValueRow, stat.ValueTable, valueBitCount, statBlock.Context);
            }
        }

        private static void _WriteStatParamData(BitBuffer bitBuffer, int bitCount, int paramShift, bool paramIsOffset, Xls.TableCodes tableCode)
        {
            if (bitCount <= 0)
            {
                bitBuffer.WriteBool(false);
                return;
            }

            bitBuffer.WriteBool(true);

            bitBuffer.WriteBits(bitCount, 6);

            int paramOperationsFlags = 0;
            if (paramShift > 0) paramOperationsFlags |= 0x01;
            if (paramIsOffset) paramOperationsFlags |= 0x02;

            bitBuffer.WriteBits(paramOperationsFlags, 2);
            if ((paramOperationsFlags & 0x01) != 0)
            {
                bitBuffer.WriteBits(paramShift, 3);
            }
            if ((paramOperationsFlags & 0x02) != 0)
            {
                bitBuffer.WriteBool(paramIsOffset);
            }

            bool hasTableCode = (tableCode != Xls.TableCodes.Null && tableCode != Xls.TableCodes.None);

            bitBuffer.WriteBool(!hasTableCode);
            if (hasTableCode)
            {
                bitBuffer.WriteUInt16((uint)tableCode);
            }
        }

        private void _WriteStatValue(BitBuffer bitBuffer, int value, Object valueRow, ExcelFile valueTable, int bitCount, StatContext statContext)
        {
            if (bitCount <= 0) return;

            if (valueTable == null)
            {
                bitBuffer.WriteBits(value, bitCount);
                return;
            }

            if (valueRow == null)
            {
                bitBuffer.WriteBits(0, bitCount);
                return;
            }

            if (statContext == StatContext.UseRows)
            {
                int rowIndex = valueTable.Rows.IndexOf(valueRow);
                bitBuffer.WriteBits(rowIndex, bitCount);
            }
            else
            {
                int code = _fileManager.GetCodeFromRow(valueTable.TableCode, valueRow);
                bitBuffer.WriteBits(code, bitCount);
            }
        }

    }
}
