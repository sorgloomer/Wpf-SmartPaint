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

        private ObservableCollection<Patch> patches;
        public ObservableCollection<Patch> Patches
        {
            get { return this.patches; }
            set
            {
                if (this.patches != value)
                {
                    this.patches = value;
                    this.NotifyPropertyChanged("Patches");
                }
            }
        }

        public Project() 
        {
            this.Patches = new ObservableCollection<Patch>();
        }
    }
}
