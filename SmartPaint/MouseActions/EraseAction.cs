using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartPaint.MouseActions
{
    public class EraseAction : IMouseAction
    {
        public Model.Project Project { get; set; }
        public double Radius { get; set; }
        public Point LastMousePosition { get; set; }

        public WriteableBitmap wbitmap;
        private Vector delta;
        private byte[] blankPixels;
        private int blankWidth;

        public EraseAction() : this(null)
        {
        }
        public EraseAction(Project project)
        {
            this.Project = project;
            this.Radius = 5;
        }

        public void MouseLeftDown(Point position)
        {
            var sel = this.Project.Patches.FirstOrDefault(p => p.Selected);
            if (sel != null)
            {
                this.delta = new Vector(sel.PositionX, sel.PositionY);
                this.wbitmap = sel.GetWriteableBitmap();
                this.blankWidth = (int)(this.Radius * 2);
                // Erasing in WPF is more like a joke...
                // Here we're just allocating an empty array. Because we have to have an empty array.
                this.blankPixels = new byte[this.blankWidth * this.blankWidth * 4];
                this.LastMousePosition = position;
            }
        }

        public void MouseLeftUp(Point position)
        {
            this.wbitmap = null;
            this.LastMousePosition = position;
        }

        public void MouseMove(Point position)
        {
            if (this.wbitmap != null)
            {
                this.wbitmap.WritePixels(
                    new Int32Rect(0, 0, this.blankWidth, this.blankWidth),
                    this.blankPixels,
                    this.blankWidth * 4,
                    (int)(position.X - this.Radius),
                    (int)(position.Y - this.Radius));
            }
        }
    }
}
