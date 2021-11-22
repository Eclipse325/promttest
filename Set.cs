using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PromtTest
{
    class Set
    {
        public List<Word>[] strings;
        public List<Range> range;


        public Set()
        {
            strings = new List<Word>[2];
            strings[0] = new List<Word>();
            strings[1] = new List<Word>();
            range = new List<Range>();
        }

        public static void print( string filename, List<Set> list) 
        {
            string tt = "\r\n";
            byte[] inf = new UTF8Encoding(true).GetBytes(tt);

            string result = "";
                foreach (Set set in list)
                {
                    for (int i = 0; i < 2; i++) {
                        foreach (Word item in set.strings[i])
                        {
                            if (item.number != -1)
                            {
                            result += "[" + Convert.ToString(item.number) + "]" + item.word + "[\\" + Convert.ToString(item.number) + "]";
                            }
                            else
                            {
                                result +=item.word;
                            }
                            if(item!= set.strings[i].FindLast(element => element.word!=string.Empty))
                                result += " ";
                        }
                        result += Environment.NewLine;
                    }
                result += Environment.NewLine;
                result += Environment.NewLine;
            }
                File.WriteAllText(filename, result);
        }

        public static List<Set> read(string path, List<Set> list)
        { 
            string[] bars = new string[] { "|||" };
            char space = ' ' ;

            string[] result;
            list = new List<Set>();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    Set str = new Set();
                    result = line.Split(bars, StringSplitOptions.None);

                    string[] temp = result[0].Split(space);
                    foreach (string str1 in temp)
                    {
                        Word item = new Word(str1);
                        str.strings[0].Add(item);
                    }

                    temp = result[1].Split(space);
                    foreach (string str1 in temp)
                    {
                        Word item = new Word(str1);
                        str.strings[1].Add(item);
                    }

                    if (str.strings[1][0].word.Equals(string.Empty))
                        str.strings[1].RemoveAt(0);

                    temp = result[2].Split(space);
                    foreach (string str1 in temp)
                    {
                        if (!str1.Equals(string.Empty))
                        {
                            Range item = new Range(str1);
                            str.range.Add(item);
                        }
                    }

                    list.Add(str);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("StackTrace: '{0}'", Environment.StackTrace);
            }
            return list;
        }

        public static List<Set> number(List<Set> list)
        {
            foreach (Set set in list)
            {
                for (int i = 0; i < set.range.Count; i++)
                {
                    set.strings[0][set.range[i].original].tags.Add(i);
                    set.strings[1][set.range[i].translated].tags.Add(i);

                    for (int j = i + 1; j < set.range.Count; j++) 
                    {
                        if (set.range[i].original == set.range[j].original)
                        {
                            set.strings[1][set.range[j].translated].tags.Add(i);
                            set.range.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }
            return list;
        }

        public static List<Set> clear(List<Set> list)
        {
            foreach (Set set in list)
            {
                for (int a = 0; a < 2; a++)
                {
                    for (int i = 0; i < set.strings[a].Count; i++)
                    {
                        set.strings[a][i].word = set.strings[a][i].word.Trim();

                        if (set.strings[a][i].word.Equals(string.Empty))
                        {
                            set.strings[a].RemoveAt(i);
                        }
                        else if (Char.IsPunctuation(set.strings[a][i].word, 0))
                        {
                            set.strings[a][i - 1].word += set.strings[a][i].word;
                            set.strings[a][i - 1].word = set.strings[a][i - 1].word.Trim();
                            set.strings[a].RemoveAt(i);
                            i--;
                        }
                        if (i < set.strings[a].Count - 1 && set.strings[a][i].number!=-1 && set.strings[a][i].number == set.strings[a][i + 1].number)
                        {
                            set.strings[a][i].word += " " + set.strings[a][i + 1].word;
                            set.strings[a][i].word = set.strings[a][i].word.Trim();
                            set.strings[a].RemoveAt(i + 1);
                        }

                    }
                }
            }
                    return list;
        }

        public bool contains(List<int> numbers, int newNumber)
        {
            bool res = false;
            if (numbers != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    foreach (Word word in this.strings[i])
                    {
                        foreach (int number in numbers)
                        {
                            if (word.tags != null)
                                foreach (int num in word.tags)
                                {
                                    if (number != -1 && num == number)
                                    {
                                        word.number = newNumber;
                                        word.tags = null;
                                        res = true;
                                    }
                                }
                        }
                    }
                }
                return res;
            }
             
            return res;
        }
        public static List<Set> reNumber(List<Set> list)
        {
            foreach (Set set in list)
            {
                int j = 0;
                for (int a = 0; a < 2; a++)
                {
                    foreach (Word word in set.strings[a])
                    {
                        if (set.contains(word.tags, j)) 
                            j++;
                    }
                }
            }

                return list;
        }
        public static List<Set> applyTokens(List<Set> list)
        {
            foreach (Set set in list)
            {
                for (int a = 0; a < 2; a++)
                {
                    for (int i = 1; i < set.strings[a].Count; i++)
                    {
                        if (set.strings[a][i].word.Contains("￨C"))
                        {
                            char[] temp = set.strings[a][i - 1].word.ToCharArray();
                            temp[0] = Convert.ToChar(Convert.ToInt32(temp[0]) - 32);
                            set.strings[a][i - 1].word = new string(temp);

                            if (set.strings[a][i].word.Length == 3) // знак пунктуации 
                            {
                                string punct = set.strings[a][i].word.Substring(2);
                                set.strings[a][i - 1].word += punct;
                            }

                            set.strings[a].RemoveAt(i);
                        }

                        if (set.strings[a][i].word.Contains("￨U"))
                        {
                            set.strings[a][i - 1].word = set.strings[a][i - 1].word.ToUpper();

                            if (set.strings[a][i].word.Length == 3) // знак пунктуации 
                            {
                                string punct = set.strings[a][i].word.Substring(2);
                                set.strings[a][i - 1].word += punct;
                            }

                            set.strings[a].RemoveAt(i);
                        }

                        if (set.strings[a][i].word.Contains("@-@"))
                        {
                            string str = "-";
                            set.strings[a][i - 1].word += str;
                            set.strings[a][i - 1].word += set.strings[a][i + 1].word;
                            set.strings[a][i - 1].tags.AddRange(set.strings[a][i + 1].tags);
                            set.strings[a].RemoveAt(i);
                            set.strings[a].RemoveAt(i);
                            i -= 2;
                        }
                    }
                    for (int i = 0; i < set.strings[a].Count; i++)
                    {
                        if (set.strings[a][i].word.Contains("@@"))
                        {
                            set.strings[a][i].word = set.strings[a][i].word.Substring(0, set.strings[a][i].word.IndexOf("@@"));
                            set.strings[a][i].word += set.strings[a][i + 1].word;
                            set.strings[a][i].tags.AddRange(set.strings[a][i + 1].tags);
                            set.strings[a].RemoveAt(i + 1);
                            i--;
                        }
                    }
                }
            }
            return list;
        }

        public void clearRange()
        {
                for (int i = 0; i < range.Count; i++)
                {
                    if (range[i].original > strings[0].Count)
                    {
                        range.RemoveAt(i);
                        i--;
                    }
                    if (range[i].translated > strings[1].Count)
                    {
                        range.RemoveAt(i);
                        i--;
                    }
                }
        }

    }
}
