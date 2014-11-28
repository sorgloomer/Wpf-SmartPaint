using SmartPaint.Model;
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
        public Project currentProject { get; set; }
        public PluginContainer Plugins { get; protected set; }
        public ApplicationContext()
        {
            this.Plugins = new PluginContainer();
            StaticLogger.Instance.OnWarn += this.ShowWarning;
            StaticLogger.Instance.OnInfo += this.ShowInfo;
            StaticLogger.Instance.OnError += this.ShowError;
            StaticLogger.Instance.LogLevel = 1;

            //TODO: ezt kitalálni, hogy legyen
            currentProject = new Project();
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


        public void OpenProjectDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            SetProjectDialog(dlg);
            Nullable<bool> result = dlg.ShowDialog();

            //TODO: if (result) {actually open project}
        }

        public void CreateProjectDialog()
        {
            /*Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            SetProjectDialog(dlg);
            Nullable<bool> result = dlg.ShowDialog();*/

            //TODO: if (result) {actually create project}

            //TODO: ezt kitalálni, hogy legyen
            currentProject = new Project();
        }

        public void ImportPictureDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            SetPictureDialog(dlg);
            Nullable<bool> result = dlg.ShowDialog();

            //TODO: if (result) {actually import}
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

        private static void SetProjectDialog(Microsoft.Win32.FileDialog dlg)
        {
            dlg.FileName = "MyProject";
            dlg.DefaultExt = ".spt";
            dlg.Filter = "Smart Paint project files (.spt)|*.spt";
        }

        private static void SetPictureDialog(Microsoft.Win32.FileDialog dlg)
        {
            dlg.FileName = "Picture";
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG image files (.png)|*.png";
        }



        internal void ExportPictureDialog()
        {
            //TODO: export to .png
            throw new NotImplementedException();
        }
    }
}
