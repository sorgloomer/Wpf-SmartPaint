using SmartPaint.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartPaint.Common
{
    public class ApplicationContext : IDisposable
    {
        public PluginContainer Plugins { get; protected set; }
        public ApplicationContext()
        {
            this.Plugins = new PluginContainer();
            StaticLogger.Instance.OnWarn += this.ShowWarning;
            StaticLogger.Instance.OnInfo += this.ShowInfo;
            StaticLogger.Instance.OnError += this.ShowError;
        }

        public void OnLoad()
        {
            var ac = new PluginContainer();
            ac.LoadPluginsDirectory();
            foreach (var s in ac.Transforms.Select(t => t.PrintableName))
            {
                StaticLogger.Info(s);
            }
        }

        public void ShowWarning(string msg)
        {
            MessageBox.Show(msg, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public void ShowInfo(string msg)
        {
            MessageBox.Show(msg, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void ShowError(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected virtual void Dispose(bool disposing)
        {
            StaticLogger.Instance.OnWarn -= this.ShowWarning;
            StaticLogger.Instance.OnInfo -= this.ShowInfo;
            StaticLogger.Instance.OnError -= this.ShowError;
        }

        ~ApplicationContext()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        public static ApplicationContext Instance = new ApplicationContext();
    }
}
