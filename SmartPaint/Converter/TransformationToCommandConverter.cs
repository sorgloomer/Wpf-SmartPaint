using SmartPaint.Common;
using SmartPaint.Utils;
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
                try
                {
                    return this.Transformation.CanApply(ApplicationContext.Instance.ViewModel.Project);
                }
                catch (Exception)
                {
                    StaticLogger.Error(String.Format("Error occured while communicating with plugin '{0}' (CanExecute).", this.Transformation.PrintableName));
                }
                return false;
            }

            public void Execute(object parameter)
            {
                try
                {
                    this.Transformation.Apply(ApplicationContext.Instance.ViewModel.Project);
                }
                catch (Exception)
                {
                    StaticLogger.Error(String.Format("Error occured while applying with plugin '{0}'.", this.Transformation.PrintableName));
                }
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
