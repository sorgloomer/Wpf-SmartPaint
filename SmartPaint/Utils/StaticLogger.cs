using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPaint.Utils
{
    public class StaticLogger
    {
        public static Logger Instance = new Logger();
        public static void Info(string msg)
        {
            Instance.Info(msg);
        }
        public static void Warn(string msg)
        {
            Instance.Warn(msg);
        }
        public static void Error(string msg)
        {
            Instance.Error(msg);
        }
    }
}
