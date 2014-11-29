using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SmartPaint.Model
{
    public class Patch
    {
        public bool Selected { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string Name { get; set; }
        public BitmapSource Image { get; set; }

        public Patch(string name, BitmapSource image, int posX, int posY) 
        {
            this.Name = name;
            this.Image = image;
            this.PositionX = posX;
            this.PositionY = posY;
        }
    }
}
