using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reanimator
{
    interface IMdiChildBase
    {
        void Save();
        void SaveAs();
        void Import();
        void Export();
    }
}
