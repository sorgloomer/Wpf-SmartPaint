using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPaint.Common
{
    public interface ITransformation
    {
        string PrintableName { get; }
        void Apply(Project project);
        bool CanApply(Project project);
    }
}
