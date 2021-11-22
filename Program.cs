using System;
using System.Collections.Generic;
using System.Text;

namespace PromtTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string inpath = "C:\\Input.txt";
            string outpath = "Result.html";

            List<Set> list = new List<Set>();
            try
            {
                inpath = args[0];
                outpath = args[1];
            }
            catch (Exception)
            {
                Console.WriteLine("StackTrace: \r\n '{0}'", Environment.StackTrace);
            }

            list = Set.read(inpath, list);
            list = Set.number(list);
            list = Set.applyTokens(list);
            list = Set.reNumber(list);
            list = Set.clear(list);
            Set.print(outpath, list);
        }
    }
}
