using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodMatchTester
{
    public class Logger
    {
        
        public static void Log(string text)
        {
            File.AppendAllText("logs.txt", text + Environment.NewLine);
        }

        //Clear logs file
        public static void ClearTextFile()
        {
            File.WriteAllText("logs.txt", string.Empty);
        }
    }
}
