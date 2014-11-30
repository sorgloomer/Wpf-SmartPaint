using SmartPaint.Common;
using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DefaultOperations
{
    public class DetectPatches : ITransformation
    {
        public bool CanApply(Project project)
        {
            return true;
        }

        public string PrintableName
        {
            get { return "Detect Patches"; }
        }


        private Project project;
        private List<Patch> selectedPatches;
        private List<Patch> generatedPatches;
        private int width, height, stride;
        private bool[] visited;
        private byte[] pixeldata;
        private int xmin, xmax, ymin, ymax;
        private int patchcounter;
        private List<I32Point> pointlist;
        private Patch patch;

        private const int OpacityTreshold = 1;

        private static int MakeStride(int width)
        {
            return ((width * 4 + 7) / 8) * 8;
        }
        protected void ProcessPoint(I32Point p)
        {
            var vi = p.Y * this.width + p.X;
            if (p.X >= 0 
                && p.X < this.width 
                && p.Y >= 0 
                && p.Y < this.height
                && !this.visited[vi]
                && this.pixeldata[p.Y * this.stride + p.X * 4 + 3] >= OpacityTreshold)
            {
                this.visited[vi] = true;
                this.xmax = Math.Max(this.xmax, p.X);
                this.xmin = Math.Min(this.xmin, p.X);
                this.ymax = Math.Max(this.ymax, p.Y);
                this.ymin = Math.Min(this.ymin, p.Y);
                this.pointlist.Add(p);
            }
        }

        protected void VisitPatch(int x, int y)
        {
            int w = this.width, h = this.height, stride = this.stride;
            this.visited[y * w + x] = true;

            this.pointlist = new List<I32Point>();
            var queuehead = 0;
            this.xmin = this.xmax = x;
            this.ymin = this.ymax = y;
            this.pointlist.Add(new I32Point(x, y));

            while (this.pointlist.Count > queuehead)
            {
                var p = this.pointlist[queuehead++];
                this.ProcessPoint(p.Add(1, 0));
                this.ProcessPoint(p.Add(-1, 0));
                this.ProcessPoint(p.Add(0, 1));
                this.ProcessPoint(p.Add(0, -1));
            }

            this.SaveNewPatch();
        }

        private void SaveNewPatch()
        {
            var neww = this.xmax - this.xmin + 1;
            var newh = this.ymax - this.ymin + 1;
            var stride = MakeStride(neww);
            var wbitmap = new WriteableBitmap(neww, newh, 96, 96, PixelFormats.Pbgra32, null);

            var data = new byte[newh * stride];

            foreach (var item in this.pointlist)
            {
                var dsti = (item.Y - this.ymin) * stride + (item.X - this.xmin) * 4;
                var srci = item.Y * this.stride + item.X * 4;
                data[dsti + 0] = this.pixeldata[srci + 0];
                data[dsti + 1] = this.pixeldata[srci + 1];
                data[dsti + 2] = this.pixeldata[srci + 2];
                data[dsti + 3] = this.pixeldata[srci + 3];
            }

            wbitmap.WritePixels(new Int32Rect(0, 0, neww, newh), data, stride, 0);

            var p = new Patch(this.patch.Name + " (" 
                + (++this.patchcounter).ToString() + ")", 
                wbitmap, 
                this.patch.PositionX + this.xmin,
                this.patch.PositionY + this.ymin);
            this.generatedPatches.Add(p);
        }
        protected void SearchPatches()
        {
            int w = this.width, h = this.height, stride = this.stride;
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    if (this.pixeldata[y * stride + x * 4 + 3] >= OpacityTreshold && !this.visited[y * w + x])
                    {
                        this.VisitPatch(x, y);
                    }
                }
            }
        }
        protected void ProcessInputPatch(Patch p)
        {
            var wbitmap = p.GetWriteableBitmap();
            this.patch = p;
            this.patchcounter = 0;
            this.width = wbitmap.PixelWidth;
            this.height = wbitmap.PixelHeight;
            this.stride = MakeStride(this.width);
            this.visited = new bool[this.height * this.width];
            this.pixeldata = new byte[this.height * this.stride];
            wbitmap.CopyPixels(this.pixeldata, this.stride, 0);
            this.SearchPatches();
            this.pixeldata = null;
            this.visited = null;
            this.patch = null;
        }
        protected List<Patch> BuildNewPatches()
        {
            this.generatedPatches = new List<Patch>();
            foreach (var item in this.selectedPatches)
            {
                this.ProcessInputPatch(item);
            }

            var result = this.generatedPatches;
            this.generatedPatches = null;
            return result;
        }

        public void Apply(Project project)
        {
            try
            {
                this.project = project;
                this.selectedPatches = project.Patches.Where(p => p.Selected).ToList();
                if (this.selectedPatches.Count() > 0)
                {
                    var patches = this.BuildNewPatches();
                    var newPatches = this.project.Patches.Where(p => !p.Selected).ToList();
                    foreach (var item in patches) item.Selected = true;
                    newPatches.AddRange(patches);
                    project.Patches = newPatches;
                }
            }
            finally
            {
                this.project = null;
            }
        }


        protected struct I32Point
        {
            public int X, Y;
            public I32Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
            public I32Point Add(int x, int y)
            {
                return new I32Point(this.X + x, this.Y + y);
            }
        }
    }
}
