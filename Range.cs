using System;
using System.Collections.Generic;
using System.Text;

namespace PromtTest
{
    class Range
    {
        public int original;
        public int translated;

        public Range(string str)
        {
            string[] temp = str.Split('-');

            original = Convert.ToInt32(temp[0]);
            translated = Convert.ToInt32(temp[1]);
            
        }
    }
}
