using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlybird.Debugger
{
    internal class Logger
    {
        public static void Log(string message)
        {
            using (StreamWriter writer = new StreamWriter("log.txt", true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
