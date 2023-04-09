﻿using ClassLibrary;
using Microsoft.Win32;
using System;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewData viewData = new ViewData();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewData;
            comboBox_Enum.ItemsSource = Enum.GetValues(typeof(FRawEnum));
            comboBox_Enum.SelectedIndex = 0;
        }
        private void LoadDataFromControls_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewData.loadFromControls();
                int result = viewData.computeSpline();
                FillUIWithData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                    viewData.rawData.Save(saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                    viewData.loadFromFile(openFileDialog.FileName);
                int result = viewData.computeSpline();
                FillUIWithData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillUIWithData()
        {
            try
            {
                rawDataListBox.ItemsSource = viewData.rawData;
                splineDataListBox.ItemsSource = viewData.splineData.SplineItemList;
                integralTextBlock.Text = viewData.splineData.Integral.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
