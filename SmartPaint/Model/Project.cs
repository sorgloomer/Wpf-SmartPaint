using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPaint.Model
{
    class Project
    {
        public LinkedList<Patch> patches { get; set; }
        public Project() 
        {
            patches = new LinkedList<Patch>();
        }
    }
}
