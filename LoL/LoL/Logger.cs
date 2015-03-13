using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoL
{
    class Logger
    {
        public static void Append(string text)
        {
            using (StreamWriter sw = new StreamWriter("LoL.log", true))
            {
                sw.WriteLine(text);
            }
        }
    }
}
