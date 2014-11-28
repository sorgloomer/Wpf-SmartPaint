using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPaint.Model
{
    public class Project
    {
        public ObservableCollection<Patch> Patches { get; set; }
        public Project() 
        {
            Patches = new ObservableCollection<Patch>();
        }
    }
}
