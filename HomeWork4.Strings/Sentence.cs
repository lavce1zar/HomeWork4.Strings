using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork4.Strings
{
    public class Sentence
    {
        public string Content { get; }

        public int CountOfSymbols { get; }

        public int CountOfWords { get; set; }

        public Sentence(string str)
        {
            Content = str;
            CountOfSymbols = str.Length;
        }
    }
}
