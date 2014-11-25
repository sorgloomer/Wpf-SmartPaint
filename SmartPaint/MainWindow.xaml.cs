﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public MainWindow()
        {
            /*System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo("hu-HU");*/
            InitializeComponent();
        }

        //TODO: no hardcoded strings! I am not sure it is a good idea to create UI from code.
        private void OpenProjectClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "MyProject";
            dlg.DefaultExt = ".spt";
            dlg.Filter = "Smart Paint project files (.spt)|*.spt";
            Nullable<bool> result = dlg.ShowDialog();

            //TODO: if (result) {actually open project}
        }

        private void CreateProjectClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "MyProject";
            dlg.DefaultExt = ".spt";
            dlg.Filter = "Smart Paint project files (.spt)|*.spt"; 
            Nullable<bool> result = dlg.ShowDialog();

            //TODO: if (result) {actually create project}
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Created by:\nHegedűs Tamás László\nDusza Andrea", SmartPaint.Properties.Resources.About, new MessageBoxButton(), MessageBoxImage.Information);
        }
    }
}
