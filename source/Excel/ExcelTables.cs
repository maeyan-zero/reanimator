using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Reanimator.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ExcelTables_Table
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] unknown;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szStringId;
        public Int16 id; 
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TableIndex
    {
        public int flag;
        public int[] indicies;
    }

    public class ExcelTables : ExcelTable
    {
        List<ExcelTables_Table> tables;
        TableIndex tableIndex;
        Stats stats;
        public Stats Stats
        {
            get
            {
                return this.stats;
            }
        }
        States states;

        public ExcelTables(byte[] data) : base(data)
        {
            tables = ReadTables<ExcelTables_Table>(data, ref offset, Count);
    
            tableIndex = (TableIndex)FileTools.ByteArrayToStructure(data, typeof(TableIndex), offset);
            offset += sizeof(Int32);
            tableIndex.indicies = FileTools.ByteArrayToInt32Array(data, offset, Count);

            // 2x more table index type chunks - check if 1 is the no. of elements in the file or something
        }

        public static List<T> ReadTables<T>(byte[] data, ref int offset, int count)
        {
            List<T> tables = new List<T>();

            for (int i = 0; i < count; i++)
            {
                T table = (T)FileTools.ByteArrayToStructure(data, typeof(T), offset);
                offset += Marshal.SizeOf(typeof(T));

                tables.Add(table);
            }

            return tables;
        }

        public string GetTableStringId(int index)
        {
            return tables[index].szStringId;
        }

        public bool LoadTables(string szFolder)
        {
            for (int i = 0; i < Count; i++)
            {
                string szStringId = GetTableStringId(i);
                string szFileName = szFolder + "\\" + szStringId + ".txt.cooked";
                FileStream cookedFile;

                try
                {
                    cookedFile = new FileStream(szFileName, FileMode.Open);
                }
                catch (Exception)
                {
                    try
                    {
                        szFileName = szFileName.Replace("_common", "");
                        cookedFile = new FileStream(szFileName, FileMode.Open);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                if (szStringId.Equals("STATS", StringComparison.OrdinalIgnoreCase))
                {
                    stats = new Stats(FileTools.StreamToByteArray(cookedFile));
                }
                else if (szStringId.Equals("STATES", StringComparison.OrdinalIgnoreCase))
                {
                    states = new States(FileTools.StreamToByteArray(cookedFile));
                }

                if (cookedFile != null)
                {
                    cookedFile.Dispose();
                }
            }

            return true;
        }
    }
}
