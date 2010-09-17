using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BudgetTextureMips
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        public Int32 group;
        public Single diffuse;
        public Single normal;
        public Single selfIllum;
        public Single diffuse2;
        public Single specular;
        public Single envMap;
        public Single lightMap;
    }
}