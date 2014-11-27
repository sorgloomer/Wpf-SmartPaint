using SmartPaint.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoOperation
{
    public class NoOperationPlugin : ITransform
    {
        public string PrintableName
        {
            get { return "NoOperation"; }
        }

        public void Apply(object doc, IEnumerable<object> patches)
        {
        }
    }
}
