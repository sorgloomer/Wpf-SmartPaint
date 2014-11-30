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

        public string PrintableName
        {
            get { return "Merge Patches"; }
        }


        private Project project;
        private List<Patch> selectedPatches;

        protected List<Patch> BuildNewPatches()
        {
            var newPatch = SmartPaint.Common.MergePatches.DoMerge(this.selectedPatches);
            newPatch.Selected = true;
            var myPatchList = this.project.Patches.ToList();
            var idx = myPatchList.IndexOf(this.selectedPatches[0]);
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
                    var patches = this.BuildNewPatches();
                    project.Patches = new ObservableCollection<Patch>(patches);
                }
            }
            finally
            {
                this.project = null;
            }
        }
    }
}
