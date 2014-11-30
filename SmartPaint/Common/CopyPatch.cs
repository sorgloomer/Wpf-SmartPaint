using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SmartPaint.Common
{
    public class CopyPatch
    {
        public static Patch Copy(Patch p)
        {
            return new Patch("Copy of " + p.Name, 
                new WriteableBitmap(p.Image),
                p.PositionX + 10, p.PositionY + 5);
        }
    }
}
