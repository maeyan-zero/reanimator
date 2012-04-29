using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemEventBeta
    {
        ExcelFile.RowHeader header;
        public Int32 level;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 levelUpItem_Name;
        public Int32 levelUpItem_Amount;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 levelUpItem_SendID;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 levelUpItem_MailSubject;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 LevelUpItem_MailBody;
        public Int32 rank;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 rankUpItem_Name;
        public Int32 rankUpItem_Amount;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 rankUpItem_SendID;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 rankUpItem_MailSubject;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 rankUpItem_MailBody;
    }
}
