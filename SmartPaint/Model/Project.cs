using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPaint.Model
{
    public class Project : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<Patch> patches;
        public List<Patch> Patches
        {
            get { return this.patches; }
            set
            {
                if (this.patches != value)
                {
                    if (this.patches != null)
                    {
                        foreach (var item in this.patches)
                        {
                            item.PropertyChanged -= this.PatchPropertyChanged;
                        }
                    }
                    this.patches = value;
                    if (this.patches != null)
                    {
                        foreach (var item in this.patches)
                        {
                            item.PropertyChanged += this.PatchPropertyChanged;
                        }
                    }
                    this.NotifyPropertyChanged("Patches");
                }
            }
        }

        private void PatchPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected")
            {
                this.NotifySelectionChanged();
            }
        }

        private void NotifySelectionChanged()
        {
            var e = this.SelectionChanged;
            if (e != null) e();
        }

        public Project() 
        {
            this.Patches = new List<Patch>();
        }


        public event Action SelectionChanged;
    }
}
