using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartPaint.MouseActions
{
    public interface IMouseAction
    {
        Project Project { get; set; }
        void MouseLeftDown(Point position);
        void MouseLeftUp(Point position);
        void MouseMove(Point position);
        void Abort();
    }
}
