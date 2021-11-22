using System;
using System.Collections.Generic;
using System.Text;

namespace PromtTest
{
    class Word
    {
        public string word;
        public int number;
        public List<int> tags;

        public Word(string w)
        {
            word = w;
            number = -1;
            tags = new List<int>();
        }
    }
}
