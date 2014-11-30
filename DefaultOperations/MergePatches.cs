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
    public class MergePatches : ITransformation
    {
        public bool CanApply(Project project)
        {
            return true;
        }

        public static BitmapSource CreateBitmap(
            int width, int height, double dpi, Action<DrawingContext> render)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                render(drawingContext);
            }
            RenderTargetBitmap bitmap = new RenderTargetBitmap(
                width, height, dpi, dpi, PixelFormats.Default);
            bitmap.Render(drawingVisual);

            return bitmap;
        }

        public string PrintableName
        {
            get { return "Merge Patches"; }
        }


        private int rectX, rectY, rectR, rectB;
        private BitmapSource wbitmap;
        private List<Patch> selectedPatches;
        private Project project;

        protected void DetermineBounds()
        {
            this.rectX = this.selectedPatches.Min(p => p.PositionX);
            this.rectY = this.selectedPatches.Min(p => p.PositionY);
            this.rectR = this.selectedPatches.Max(p => p.PositionX + p.Image.PixelWidth);
            this.rectB = this.selectedPatches.Max(p => p.PositionY + p.Image.PixelHeight);
        }

        protected void DrawPatches(DrawingContext dc)
        {
            foreach (var patch in this.selectedPatches)
            {
                dc.DrawImage(patch.Image, new Rect(
                    patch.PositionX - this.rectX, 
                    patch.PositionY - this.rectY,
                    patch.Image.PixelWidth,
                    patch.Image.PixelHeight));
            }
        }
        protected List<Patch> BuildNewPatches()
        {
            var name = this.selectedPatches.First().Name;
            var newPatch = new Patch(name, this.wbitmap, this.rectX, this.rectY);
            var myPatchList = this.project.Patches.ToList();
            var idx = myPatchList.IndexOf(selectedPatches.First());
            myPatchList = myPatchList.Where(p => !p.Selected).ToList();
            myPatchList.Insert(idx, newPatch);
            return myPatchList;
        }

        public void Apply(Project project)
        {
            try
            {
                this.project = project;
                this.selectedPatches = project.Patches.Where(p => p.Selected).ToList();
                if (this.selectedPatches.Count() > 0)
                {
                    this.DetermineBounds();
                    this.wbitmap = CreateBitmap(this.rectR - this.rectX, this.rectB - this.rectY, 96, this.DrawPatches);
                    var patches = this.BuildNewPatches();
                    project.Patches = new List<Patch>(patches);
                }
            }
            finally
            {
                this.wbitmap = null;
                this.project = null;
            }
        }
    }
}
