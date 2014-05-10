using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SheetEditor
{
    class Formula
    {
        public static string Calculate(string input)
        {
            return input;
        }

        public static string ColumnName(int x)
        {
            string name = "";
            int v = x;
            while(v > 0)
            {
                v -= 1;
                int c = (int)'A' + (v % 26);
                name = (char)c + name;
                v /= 26;
            }
            return name;
        }
    }
}
