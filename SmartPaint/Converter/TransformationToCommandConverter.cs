using SmartPaint.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SmartPaint.Converter
{
    public class TransformationToCommandConverter : IValueConverter
    {
        public class TransformationCommand : ICommand
        {
            public ITransformation Transformation { get; set; }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return this.Transformation.CanApply(ApplicationContext.Instance.ViewModel.Project);
            }

            public void Execute(object parameter)
            {
                this.Transformation.Apply(ApplicationContext.Instance.ViewModel.Project);
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ITransformation)
            {
                return new TransformationCommand() { Transformation = (ITransformation)value };
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
