using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SmartPaint.Common
{
    public class MergePatches
    {
        private int rectX, rectY, rectR, rectB;
        private BitmapSource wbitmap;
        private List<Patch> selectedPatches;

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

        public Patch Merge(List<Patch> patches)
        {

            return patches.Count > 0 ? Merge(patches, patches[0].Name) : null;
        }

        public Patch Merge(List<Patch> patches, string name)
        {
            try
            {
                this.selectedPatches = patches.ToList();
                if (this.selectedPatches.Count > 0)
                {
                    this.DetermineBounds();
                    this.wbitmap = CreateBitmap(this.rectR - this.rectX, this.rectB - this.rectY, 96, this.DrawPatches);
                    return new Patch(name, this.wbitmap, this.rectX, this.rectY);
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                this.wbitmap = null;
            }
        }

        public static Patch DoMerge(List<Patch> patches, string name)
        {
            var mp = new MergePatches();
            return mp.Merge(patches, name);
        }

        public static Patch DoMerge(List<Patch> patches)
        {
            var mp = new MergePatches();
            return mp.Merge(patches);
        }
    }
}
