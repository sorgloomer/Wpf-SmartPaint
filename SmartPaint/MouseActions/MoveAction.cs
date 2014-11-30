using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartPaint.MouseActions
{
    public class MoveAction : IMouseAction
    {
        public Model.Project Project { get; set; }

        private Dictionary<Patch, System.Windows.Vector> deltaPosition;

        public MoveAction()
        {
        }
        public MoveAction(Project project)
        {
            this.Project = project;
        }

        public void MouseLeftDown(Point position)
        {
            this.deltaPosition = new Dictionary<Patch, System.Windows.Vector>();
            foreach (Patch p in this.Project.Patches)
            {
                this.deltaPosition.Add(p, new System.Windows.Point(p.PositionX, p.PositionY) - position);
            }
        }

        public void MouseLeftUp(Point position)
        {
            this.deltaPosition = null;
        }

        public void MouseMove(Point position)
        {
            if (this.deltaPosition != null)
            {
                foreach (Patch p in this.Project.Patches)
                {
                    if (p.Selected)
                    {
                        Vector delta;
                        if (this.deltaPosition.TryGetValue(p, out delta))
                        {
                            var offset = (position + delta);
                            p.PositionX = (int)offset.X;
                            p.PositionY = (int)offset.Y;
                        }
                    }
                }
            }
        }
    }
}
