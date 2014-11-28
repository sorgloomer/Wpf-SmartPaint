using SmartPaint.Model;
using SmartPaint.Utils;
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

        public MainWindow MainWindow { get; set; }
        public PluginContainer Plugins { get; protected set; }
        public ApplicationContext()
        {
            this.Plugins = new PluginContainer();
            StaticLogger.Instance.OnWarn += this.ShowWarning;
            StaticLogger.Instance.OnInfo += this.ShowInfo;
            StaticLogger.Instance.OnError += this.ShowError;
            StaticLogger.Instance.LogLevel = 1;

        }

        public void OnLoad()
        {
            var plugins = this.Plugins;
            plugins.LoadPluginsDirectory();
            foreach (var s in plugins.Transformations.Select(t => t.PrintableName))
            {
                StaticLogger.Info(s);
            }

            //TODO: ezt kitalálni, hogy legyen
            MainWindow.PatchList = new ObservableCollection<Patch>() {};
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
            MainWindow.PatchList = new ObservableCollection<Patch>() { };
        }

        public void ImportPictureDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            SetPictureDialog(dlg);
            Nullable<bool> result = dlg.ShowDialog();

            if (result!=null && result==true) 
            {
                int c = MainWindow.PatchList.Count+1;
                BitmapImage bImg = new BitmapImage(new Uri(dlg.FileName, UriKind.Absolute));
                MainWindow.PatchList.Add(new Patch("patch" + c, bImg, 0, 0));

                //TODO: It dos not allow moving, I think
                Image toCanvas = new Image();
                toCanvas.Source = bImg;
                toCanvas.Width = bImg.Width;
                toCanvas.Height = bImg.Height;
                Canvas.SetLeft(toCanvas,0);
                Canvas.SetTop(toCanvas,0);
                MainWindow.canvas.Children.Add(toCanvas);

            }
        }

        public void ExportPictureDialog()
        {
            //TODO: export to .png
            throw new NotImplementedException();
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
