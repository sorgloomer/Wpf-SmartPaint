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
    public class Erosion : ITransformation
    {
        public bool CanApply(Project project)
        {
            return true;
        }

        public string PrintableName
        {
            get { return "Erosion"; }
        }


        private Project project;

        private WriteableBitmap wbitmap;

        private byte[] pixeldata;
        private int width, height, stride;
        private bool[] mask1, mask2;

        private void CalcMask()
        {
            var mask = this.mask1;
            var data = this.pixeldata;
            var lineoffset = this.stride - width * 4;
            int cptr = 3, tptr = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    mask[tptr] = data[cptr] < 5;
                    tptr++;
                    cptr += 4;
                }
                cptr += lineoffset;
            }
        }
        private void FattenMask()
        {
            var w = this.width;
            var h = this.height;
            var stride = this.stride;
            var mask1 = this.mask1;
            var mask2 = this.mask2;
            int tptr = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    mask2[tptr] = mask1[tptr]
                        || (y > 0 && mask1[(y - 1) * w + x])
                        || (x > 0 && mask1[y * w + (x - 1)])
                        || ((y < h - 1) && mask1[(y + 1) * w + x])
                        || ((x < w - 1) && mask1[y * w + (x + 1)]);
                    tptr++;
                }
            }
        }
        private void UseMask()
        {
            var mask = this.mask2;
            var data = this.pixeldata;
            int width = this.width, height = this.height;
            var lineoffset = this.stride - width * 4;
            int cptr = 0, tptr = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (mask[tptr])
                    {
                        data[cptr + 0] = 0;
                        data[cptr + 1] = 0;
                        data[cptr + 2] = 0;
                        data[cptr + 3] = 0;
                    }
                    tptr++;
                    cptr += 4;
                }
                cptr += lineoffset;
            }
        }
        private void ErodeData()
        {
            this.CalcMask();
            this.FattenMask();
            this.UseMask();
        }


        private void ErodeWBitmap()
        {
            this.width = this.wbitmap.PixelWidth;
            this.height = this.wbitmap.PixelHeight;
            this.stride = ((this.width * 4 + 7) / 8) * 8;
            this.pixeldata = new byte[this.stride * this.height];
            this.mask1 = new bool[this.width * this.height];
            this.mask2 = new bool[this.width * this.height];
            this.wbitmap.CopyPixels(this.pixeldata, stride, 0);

            this.ErodeData();

            this.wbitmap.WritePixels(new Int32Rect(0, 0, width, height), this.pixeldata, this.stride, 0);
            this.pixeldata = null;
            this.mask1 = null;
            this.mask2 = null;
        }

        public WriteableBitmap ErodePicture(WriteableBitmap input)
        {
            this.wbitmap = input;
            this.ErodeWBitmap();
            var wi = this.wbitmap;
            this.wbitmap = null;
            return wi;
        }
        public void Apply(Project project)
        {
            this.project = project;
            foreach (var p in project.Patches.Where(p => p.Selected))
            {
                p.Image = this.ErodePicture(p.GetWriteableBitmap());
            }
        }
    }
}
