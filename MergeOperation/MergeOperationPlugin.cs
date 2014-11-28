using SmartPaint.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeOperation
{
    public class MergeOperationPlugin : ITransformation
    {
        public string PrintableName
        {
            get { return "MergeOperation"; }
        }

        public void Apply(object doc, IEnumerable<object> patches)
        {
        }
    }
}
