using ClassLibrary;
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
        public OxyPlotModel oxyPlotMod;

        public static RoutedCommand LoadFromControlsCommand = new RoutedCommand("LoadFromControls", typeof(MainWindow));
        public static RoutedCommand LoadFromFileCommand = new RoutedCommand("LoadFromFile", typeof(MainWindow));
        public static RoutedCommand ComputeSplineCommand = new RoutedCommand("ComputeSpline", typeof(MainWindow));
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
        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString());
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFile_Click(sender, e);
        }
        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !(Validation.GetHasError(RawBounds) || Validation.GetHasError(RawNumOfNodes)) && (viewData.rawData != null);
        }

        private void LoadFromControlsCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            LoadDataFromControls_Click(sender, e);
            drawSpline();
        }
        private void CanLoadFromControlsCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !(Validation.GetHasError(RawBounds) || Validation.GetHasError(RawNumOfNodes) || Validation.GetHasError(SplineNumOfNodes));
        }

        private void LoadFromFileCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                    viewData.loadFromFile(openFileDialog.FileName);
                ComputeSplineCommand.Execute(sender, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CanLoadFromFileCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !(Validation.GetHasError(SplineNumOfNodes));
        }

        private void ComputeSplineCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            int result = viewData.computeSpline();
            FillUIWithData();
            drawSpline();
        }
        private void CanComputeSplineCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            bool rb = Validation.GetHasError(RawBounds);
            bool rn = Validation.GetHasError(RawNumOfNodes);
            bool sn = Validation.GetHasError(SplineNumOfNodes);
            e.CanExecute = !(Validation.GetHasError(RawBounds) || Validation.GetHasError(RawNumOfNodes) || Validation.GetHasError(SplineNumOfNodes)) && (viewData.rawData != null);
        }

        private void drawSpline()
        {
            try
            {
                oxyPlotMod = new OxyPlotModel(viewData.splineData);
                OxyPlot.DataContext = oxyPlotMod;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"шибка отрисовки сплайна\n" + ex.Message);
            }
        }
    }
}
