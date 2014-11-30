using SmartPaint.Common;
using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmartPaint.Persistence;
using SmartPaint.ViewModel;
using System.Windows.Controls;

namespace SmartPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            
            System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo(Properties.Settings.Default.Lang);
            InitializeComponent();

            ApplicationContext.Instance.OnLoad(this);
        }

        private DocumentScope viewModel;
        public DocumentScope ViewModel
        {
            get { return viewModel; }
            set
            {
                this.viewModel = value;
                this.DataContext = value;
            }
        }

        //TODO: no hardcoded strings! I am not sure it is a good idea to create UI from code.
        private void OpenProjectClick(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Instance.OpenProjectDialog();
        }

        private void CreateProjectClick(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Instance.CreateProjectDialog();
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(SmartPaint.Properties.Resources.CreatedBy + "\nHegedűs Tamás László\nDusza Andrea", SmartPaint.Properties.Resources.About, new MessageBoxButton(), MessageBoxImage.Information);
        }

        private void SaveProjectClick(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Instance.SaveProjectDialog();
        }

        private void ImportPictureClick(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Instance.ImportPictureDialog();

        }

        private void ExportPictureClick(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Instance.ExportPictureDialog();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private Dictionary<Patch,System.Windows.Vector> mouseDistanceFromObject;

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((UIElement)sender).CaptureMouse();
            mouseDistanceFromObject = new Dictionary<Patch, System.Windows.Vector>();
            foreach (Patch p in ViewModel.Project.Patches)
            {
                mouseDistanceFromObject.Add(p,(e.GetPosition((Canvas)sender) -new System.Windows.Point(p.PositionX, p.PositionY)));
            }

        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((UIElement)sender).ReleaseMouseCapture();
            mouseDistanceFromObject = null;
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDistanceFromObject != null)
            {
                var position = e.GetPosition((Canvas)sender);


                foreach (Patch p in ViewModel.Project.Patches)
                {
                    if (p.Selected)
                    {
                        var offset = (position - mouseDistanceFromObject[p]);
                        p.PositionX = (int)offset.X;
                        p.PositionY = (int)offset.Y;

                    }
                }
            }
        }

        private void MovePatchUp(object sender, RoutedEventArgs e)
        {
            viewModel.MovePatchUp();   
        }

        private void MovePatchDown(object sender, RoutedEventArgs e)
        {
            viewModel.MovePatchDown();
        }

        private void setEnglish_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Lang = "en-US";
            Properties.Settings.Default.Save();
            System.Windows.MessageBox.Show(SmartPaint.Properties.Resources.PleaseRestart, SmartPaint.Properties.Resources.Warning, new MessageBoxButton(), MessageBoxImage.Exclamation);
        }

        private void setHungarian_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Lang = "hu-HU";
            Properties.Settings.Default.Save();
            System.Windows.MessageBox.Show(SmartPaint.Properties.Resources.PleaseRestart, SmartPaint.Properties.Resources.Warning, new MessageBoxButton(), MessageBoxImage.Exclamation);
        }

    }
}
