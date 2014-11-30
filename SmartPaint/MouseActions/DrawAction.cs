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
    public class DrawAction : IMouseAction
    {
        public Model.Project Project { get; set; }
        public Color Color { get; set; }
        public double Radius { get; set; }
        public Point LastMousePosition { get; set; }
        public RenderTargetBitmap Target { get; set; }
        private Vector delta;
        private SolidColorBrush brush;
        private Pen pen;

        public DrawAction() : this(null)
        {
        }
        public DrawAction(Project project)
        {
            this.Project = project;
            this.Color = Colors.Black;
            this.Radius = 5;
            this.brush = new SolidColorBrush(Colors.Black);
            this.pen = new Pen(this.brush, this.Radius);
        }

        public void MouseLeftDown(Point position)
        {
            var sel = this.Project.Patches.FirstOrDefault(p => p.Selected);
            if (sel != null)
            {
                this.delta = new Vector(sel.PositionX, sel.PositionY);
                this.Target = sel.GetRenderTargetBitmap();
                this.brush.Color = this.Color;
                this.pen.Thickness = this.Radius;
                this.pen.StartLineCap = this.pen.EndLineCap = PenLineCap.Round;
                this.LastMousePosition = position;
            }
        }

        public void MouseLeftUp(Point position)
        {
            this.Target = null;
            this.LastMousePosition = position;
        }

        public void MouseMove(Point position)
        {
            if (this.Target != null)
            {
                var dv = new DrawingVisual();
                var dc = dv.RenderOpen();
                dc.DrawLine(this.pen, this.LastMousePosition - this.delta, position - this.delta);
                dc.Close();

                this.Target.Render(dv);
                this.LastMousePosition = position;
            }
        }
    }
}
