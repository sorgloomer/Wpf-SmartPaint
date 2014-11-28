using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPaint.Model
{
    public class Project
    {
        public List<Patch> patches { get; set; }
        public Project() 
        {
            patches = new List<Patch>();
        }
    }
}
