using SmartPaint.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SmartPaint.Model
{
    public class Patch : INotifyPropertyChanged
    {
        /*public static readonly DependencyProperty PositionXProperty = DependencyProperty.Register("PositionX", typeof(int), typeof(Patch));
        public static readonly DependencyProperty PositionYProperty = DependencyProperty.Register("PositionY", typeof(int), typeof(Patch));*/

        private bool selected;

        public bool Selected
        {
            get { return selected; }
            set { selected = value; NotifyPropertyChanged("Selected");}
        }

        private int positionX;

        public int PositionX
        {
            get { return positionX; }
            set { positionX = value; NotifyPropertyChanged("PositionX"); }
        }

        private int positionY;

        public int PositionY
        {
            get { return positionY; }
            set { positionY = value; NotifyPropertyChanged("PositionY"); }
        }
        
        public string Name { get; set; }
        private BitmapSource image;
        public BitmapSource Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                this.NotifyPropertyChanged("Image");
            }
        }

        public WriteableBitmap GetWriteableBitmap()
        {
            if (this.Image == null)
            {
                return null;
            }
            else
            {
                if (this.Image is WriteableBitmap)
                {
                    return (WriteableBitmap)this.Image;
                }
                else
                {
                    var wbitmap = new WriteableBitmap(this.Image);
                    this.Image = wbitmap;
                    return wbitmap;
                }
            }
        }

        public Patch(string name, BitmapSource image, int posX, int posY) 
        {
            this.Name = name;
            this.Image = image;
            this.PositionX = posX;
            this.PositionY = posY;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
