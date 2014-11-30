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
    public class CopyPatches : ITransformation
    {
        public bool CanApply(Project project)
        {
            return true;
        }

        public string PrintableName
        {
            get { return "Copy Patches"; }
        }

        public void Apply(Project project)
        {
            var selected = project.Patches.Where(p => p.Selected).ToList();
            var copy = selected.Select(CopyPatch.Copy).ToList();
            foreach (var p in selected) p.Selected = false;
            foreach (var p in copy) p.Selected = true;
            var newList = new List<Patch>(project.Patches);
            newList.AddRange(copy);
            project.Patches = newList;
        }
    }
}
