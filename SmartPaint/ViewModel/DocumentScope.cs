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
                if (this.project != null) this.project.SelectionChanged -= this.OnSelectionChanged;
                this.project = value;
                if (this.project != null) this.project.SelectionChanged += this.OnSelectionChanged;
                this.NotifyPropertyChanged("Project");
            }
        }

        private void OnSelectionChanged()
        {
            this.NotifyPropertyChanged("UpMovingActive");
            this.NotifyPropertyChanged("DownMovingActive");
        }

        public DocumentScope()
        {
            this.Transformations = new ObservableCollection<ITransformation>();
            this.Project = new Project();
        }

        //TODO: notifypropertychanged
        public bool UpMovingActive
        {
            
            get { List<Patch> ps = Project.Patches.Where(p=>p.Selected).ToList();
                return (ps.Count() == 1) && (ps.ElementAt(0) != Project.Patches.ElementAt(0)); }
        }

        public bool DownMovingActive
        {
            get
            {
                List<Patch> ps = (List<Patch>)Project.Patches.Where(p => p.Selected).ToList();
                return (ps.Count() == 1) && (ps.ElementAt(0) != Project.Patches.ElementAt(Project.Patches.Count()-1));
            }
        }

        public void MovePatchUp() {
            List<Patch> newList = this.Project.Patches.ToList();
            int idx = newList.FindIndex(p => p.Selected);
            Patch toMove = newList.ElementAt(idx);
            newList.RemoveAt(idx);
            newList.Insert(idx-1,toMove);
            Project.Patches = newList;

        }
        public void MovePatchDown() {
            List<Patch> newList = this.Project.Patches.ToList();
            int idx = newList.FindIndex(p => p.Selected);
            Patch toMove = newList.ElementAt(idx);
            newList.RemoveAt(idx);
            newList.Insert(idx + 1, toMove);
            Project.Patches = newList;
        }
    }
}
