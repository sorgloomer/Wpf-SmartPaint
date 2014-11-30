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
            this.NotifyPropertyChanged("CopyPatchActive");
            this.NotifyPropertyChanged("RemovePatchActive");
        }

        public DocumentScope()
        {
            this.Transformations = new ObservableCollection<ITransformation>();
            this.Project = new Project();
        }
        public bool UpMovingActive
        {
            get
            {
                var idx = this.FirstSelectedIndex();
                return idx >= 0 && idx > 0;
            }
        }

        public bool DownMovingActive
        {
            get
            {
                var idx = this.FirstSelectedIndex();
                return idx >= 0 && idx < this.Project.Patches.Count - 1;
            }
        }
        private int FirstSelectedIndex()
        {
            return this.Project.Patches.FindIndex(p => p.Selected);
        }
        public bool RemovePatchActive
        {
            get
            {
                return this.FirstSelectedIndex() >= 0;
            }
        }
        public bool CopyPatchActive
        {
            get
            {
                return this.FirstSelectedIndex() >= 0;
            }
        }

        public void MovePatchUp() {
            var newList = this.Project.Patches;
            int idx = FirstSelectedIndex();
            if (idx >= 0 && idx > 0)
            {
                newList = newList.ToList();
                Patch toMove = newList.ElementAt(idx);
                newList.RemoveAt(idx);
                newList.Insert(idx - 1, toMove);
                Project.Patches = newList;
                this.OnSelectionChanged();
            }
        }
        public void MovePatchDown() {
            var newList = this.Project.Patches;
            int idx = FirstSelectedIndex();
            if (idx >= 0 && idx < newList.Count - 1)
            {
                newList = newList.ToList();
                Patch toMove = newList.ElementAt(idx);
                newList.RemoveAt(idx);
                newList.Insert(idx + 1, toMove);
                Project.Patches = newList;
                this.OnSelectionChanged();
            }
        }

        public void CopyPatch()
        {
            var project = this.Project;
            var selected = project.Patches.Where(p => p.Selected).ToList();
            var copy = selected.Select(SmartPaint.Common.CopyPatch.Copy).ToList();
            foreach (var p in selected) p.Selected = false;
            foreach (var p in copy) p.Selected = true;
            var newList = new List<Patch>(project.Patches);
            newList.AddRange(copy);
            project.Patches = newList;
            this.OnSelectionChanged();
        }

        public void RemovePatch()
        {
            this.Project.Patches = this.Project.Patches.Where(p => !p.Selected).ToList();
            this.OnSelectionChanged();
        }
    }
}
