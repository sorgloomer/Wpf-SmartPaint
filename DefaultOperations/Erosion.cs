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
        private List<Patch> selectedPatches;

        private WriteableBitmap wbitmap;

        private byte[] pixeldata;
        private int width, height;
        private bool[] mask;

        private void CalcMask()
        {
            var mask = this.mask;
            var data = this.pixeldata;
            int cptr = 0, tptr = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    mask[tptr] = data[cptr] < 5;
                    tptr++;
                    cptr += 4;
                }
            }
        }
        private void ErodeData()
        {
            this.CalcMask();
            //this.FattenMask();
            //this.UseMask();
        }
        public void ErodePicture()
        {
            var width = this.wbitmap.PixelWidth;
            var height = this.wbitmap.PixelHeight;
            var data = new byte[width * height * 4];
            var mask = new bool[width * height];
            this.wbitmap.CopyPixels(data, width * 4, 0);
            {
                this.width = width;
                this.height = height;
                this.pixeldata = data;
                this.mask = mask;
                {
                    this.ErodeData();
                }
                this.wbitmap.WritePixels(new Int32Rect(0, 0, width, height), this.pixeldata, width * 4, 0);
                this.pixeldata = null;
                this.mask = null;
            }
        }
    }
}
