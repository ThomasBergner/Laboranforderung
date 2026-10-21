using System;
using System.Collections.Generic;
using System.Text;

namespace Laboranforderung.Daten
{
    public class LaborwertComparer: IComparer<Laborwert>
    {
        public int Compare(Laborwert? x, Laborwert? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return x.Index.CompareTo(y.Index);
        }
    }
}
