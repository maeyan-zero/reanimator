using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AiCommonStateRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string stat;
        public short code;
    }

    // todo: FIXME
    /*
        public string GetStringFromId(int id)
        {
            foreach (AiCommonStateTable aiCommonStateTable in tables)
            {
                if (aiCommonStateTable.code == id)
                {
                    return aiCommonStateTable.stat;
                }
            }

            return "NOT FOUND";
        }*/
}