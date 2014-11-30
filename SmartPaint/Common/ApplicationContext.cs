using SmartPaint.Model;
using SmartPaint.Persistence;
using SmartPaint.Utils;
using SmartPaint.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SmartPaint.Common
{
    public class ApplicationContext : IDisposable
    {

        public DocumentScope ViewModel { get; set; }
        public MainWindow MainWindow { get; set; }
        public PluginContainer Plugins { get; protected set; }
        public ApplicationContext()
        {
            this.Plugins = new PluginContainer();
            this.ViewModel = new DocumentScope();
            StaticLogger.Instance.OnWarn += this.ShowWarning;
            StaticLogger.Instance.OnInfo += this.ShowInfo;
            StaticLogger.Instance.OnError += this.ShowError;
            StaticLogger.Instance.LogLevel = 1;

        }

        public void OnLoad(MainWindow mw)
        {
            this.Plugins.LoadPluginsDirectory();
            this.MainWindow = mw;
            this.ViewModel.Transformations = new ObservableCollection<ITransformation>(this.Plugins.Transformations);
            this.MainWindow.ViewModel = this.ViewModel;
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
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                SetProjectDialog(dlg);
                Nullable<bool> result = dlg.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    this.ViewModel.Project = SptFile.From(dlg.FileName);
                }
            }
            catch (Exception)
            {
                StaticLogger.Error("There was an error while opening the file!");
            }
        }
        public void SaveProjectDialog()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            SetProjectDialog(dlg);
            Nullable<bool> result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                SptFile.Save(dlg.FileName, this.ViewModel.Project);
            }
        }

        public void CreateProjectDialog()
        {
            /*Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            SetProjectDialog(dlg);
            Nullable<bool> result = dlg.ShowDialog();*/

            //TODO: if (result) {actually create project}

            //TODO: ezt kitalálni, hogy legyen
            this.ViewModel.Project.Patches = new List<Patch>() { };
        }

        public void ImportPictureDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            SetPictureDialog(dlg);
            Nullable<bool> result = dlg.ShowDialog();

            if (result!=null && result==true) 
            {
                int c = this.ViewModel.Project.Patches.Count+1;
                BitmapImage bImg = new BitmapImage(new Uri(dlg.FileName, UriKind.Absolute));
                var newList = this.ViewModel.Project.Patches.ToList();
                newList.Add(new Patch("patch" + c, bImg, 200, 200));
                this.ViewModel.Project.Patches = newList;
            }
        }

        public void ExportPictureDialog()
        {
            //TODO: export to .png
            //throw new NotImplementedException();
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

    }
}
