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
using SmartPaint.MouseActions;

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
            System.Threading.Thread.CurrentThread.CurrentCulture =
                new System.Globalization.CultureInfo(Properties.Settings.Default.Lang);
            InitializeComponent();
            this.EraseAction = new EraseAction();
            this.DrawAction = new DrawAction();
            this.MoveAction = new MoveAction();
            this.CurrentMouseAction = MoveAction;
            ApplicationContext.Instance.OnLoad(this);
        }

        public IMouseAction CurrentMouseAction { get; set; }
        public EraseAction EraseAction { get; set; }
        public DrawAction DrawAction { get; set; }
        public MoveAction MoveAction { get; set; }

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

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (UIElement)sender;
            canvas.CaptureMouse();
            var ma = this.CurrentMouseAction;
            if (ma != null)
            {
                ma.Project = this.ViewModel == null ? null : this.ViewModel.Project;
                var position = e.GetPosition(canvas);
                ma.MouseLeftDown(position);
            }
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var canvas = (UIElement)sender;
            canvas.ReleaseMouseCapture();
            var ma = this.CurrentMouseAction;
            if (ma != null)
            {
                var position = e.GetPosition(canvas);
                ma.MouseLeftUp(position);
            }
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            var canvas = (UIElement)sender;
            var ma = this.CurrentMouseAction;
            if (ma != null)
            {
                var position = e.GetPosition(canvas);
                ma.MouseMove(position);
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

        private void colorPicker_SelectedColorChanged_1(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color> e)
        {
            DrawAction.Color = e.NewValue;
        }

        private void MoveMode_Checked_1(object sender, RoutedEventArgs e)
        {
            CurrentMouseAction = MoveAction;
        }

        private void BrushMode_Checked_1(object sender, RoutedEventArgs e)
        {
            CurrentMouseAction = DrawAction;
        }

        private void EraseMode_Checked(object sender, RoutedEventArgs e)
        {
            CurrentMouseAction = EraseAction;
        }

    }
}
