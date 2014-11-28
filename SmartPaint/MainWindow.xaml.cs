﻿using SmartPaint.Common;
using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 

    public partial class MainWindow : Window
    {
        
        public ObservableCollection<Patch> myPatchList = new ObservableCollection<Patch>() { 
            //TODO: no hardcoding
            new Patch("alma",new BitmapImage(new Uri("c:\\Data\\Projects\\GitRepos\\Wpf-SmartPaint\\Examples\\apple.png",UriKind.Absolute)),0,0
) };

        public ObservableCollection<Patch> MyPatchList
        {
            get { return myPatchList; }
            set { myPatchList = value; }
        }


        public MainWindow()
        {
            
            /*System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo("hu-HU");*/
            InitializeComponent();
            this.DataContext = this;
            ApplicationContext.Instance.OnLoad();
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
            System.Windows.MessageBox.Show("Created by:\nHegedűs Tamás László\nDusza Andrea", SmartPaint.Properties.Resources.About, new MessageBoxButton(), MessageBoxImage.Information);
        }

        private void SaveProjectClick(object sender, RoutedEventArgs e)
        {
            //TODO: save to .spt
        }

        private void ImportPictureClick(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Instance.ImportPictureDialog();

        }

        private void ExportPictureClick(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Instance.ExportPictureDialog();
        }
    }
}
