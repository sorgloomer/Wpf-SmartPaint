using SmartPaint.Common;
using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPaint.ViewModel
{
    public class DocumentScope : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<ITransformation> transformations;
        public ObservableCollection<ITransformation> Transformations
        {
            get { return this.transformations; }
            set
            {
                if (this.transformations != value)
                {
                    this.transformations = value;
                    this.NotifyPropertyChanged("Transformations");
                }
            }
        }

        private Project project;
        public Project Project
        {
            get { return this.project; }
            set
            {
                this.project = value;
                this.NotifyPropertyChanged("Project");
            }
        }

        public DocumentScope()
        {
            this.Transformations = new ObservableCollection<ITransformation>();
            this.Project = new Project();
        }
    }
}
