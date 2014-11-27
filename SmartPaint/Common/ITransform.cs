﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPaint.Common
{
    public interface ITransform
    {
        string PrintableName { get; }
        void Apply(object doc, IEnumerable<object> patches);
    }
}
