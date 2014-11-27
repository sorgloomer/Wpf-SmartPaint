using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPaint.Utils
{
    public class Logger
    {
        public int LogLevel { get; set; }
        public event Action<string> OnInfo;
        public event Action<string> OnWarn;
        public event Action<string> OnError;
        public void Info(string msg)
        {
            if (this.LogLevel <= 0 && this.OnInfo != null) this.OnInfo(msg);
        }
        public void Warn(string msg)
        {
            if (this.LogLevel <= 1 && this.OnWarn != null) this.OnWarn(msg);
        }
        public void Error(string msg)
        {
            if (this.LogLevel <= 2 && this.OnError != null) this.OnError(msg);
        }
    }
}
