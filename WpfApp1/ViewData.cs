using ClassLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    internal class ViewData: INotifyPropertyChanged, IDataErrorInfo
    {
        private double leftBound;
        private double rightBound;
        private int rawNumOfNodes;
        private bool isUniformGrid;
        private FRawEnum fRawEnum;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public double LeftBound
        {
            get { return leftBound; }
            set
            {
                leftBound = value;
                OnPropertyChanged("LeftBound");
            }
        }
        public double RightBound
        {
            get { return rightBound; }
            set
            {
                rightBound = value;
                OnPropertyChanged("RightBound");
            }
        }
        public int RawNumOfNodes
        {
            get { return rawNumOfNodes; }
            set
            {
                rawNumOfNodes = value;
                OnPropertyChanged("RawNumOfNodes");
            }
        }
        public bool IsUniformGrid
        {
            get { return isUniformGrid; }
            set
            {
                isUniformGrid = value;
                OnPropertyChanged("IsUniformGrid");
            }
        }
        public FRawEnum FRawEnum
        {
            get { return fRawEnum; }
            set
            {
                fRawEnum = value;
                OnPropertyChanged("FRawEnum");
            }
        }
        public int SplineNumOfNodes { get; set; }
        public double LeftFirstDerivative { get; set; }
        public double RightFirstDerivative { get; set; }
        public RawData? rawData { get; set; }
        public SplineData? splineData { get; set; }

        public ViewData()
        {
            leftBound = 0;
            rightBound = 1;
            rawNumOfNodes = 2;
            SplineNumOfNodes = 2;
            isUniformGrid = true;
            rawData = new RawData(0, 1, 2, true, CreationFunctions.LinearFunc);
            splineData = null;
        }
        public void Save(string filename)
        {
            try
            {
                rawData.Save(filename);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void loadFromControls()
        {
            try
            {
                FRaw fRaw = enumToFunc(fRawEnum);
                rawData = new RawData(LeftBound, RightBound, RawNumOfNodes, IsUniformGrid, fRaw);
            }
            catch (Exception)
            {
                throw new Exception("Неправильный формат ввода!");
            }
        }
        public void loadFromFile(string filename)
        {
            try
            {
                rawData = new RawData(filename);
                LeftBound = rawData.LeftBound;
                RightBound = rawData.RightBound;
                RawNumOfNodes = rawData.NumOfNodes;
                IsUniformGrid = rawData.IsUniformGrid;
                FRawEnum = funcToEnum(rawData.fRaw);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int computeSpline()
        {
            try
            {
                splineData = new SplineData(rawData, LeftFirstDerivative, RightFirstDerivative, SplineNumOfNodes);
                return splineData.CreateSpline();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public FRaw enumToFunc(FRawEnum fRawEnum)
        {
            FRaw fRaw = fRawEnum switch
            {
                FRawEnum.LinearFunc => CreationFunctions.LinearFunc,
                FRawEnum.ThreePolynomFunc => CreationFunctions.ThreePolynomFunc,
                FRawEnum.RandomValueFunc => CreationFunctions.RandomValueFunc,
                _ => CreationFunctions.LinearFunc
            };
            return fRaw;
        }
        public FRawEnum funcToEnum(FRaw fRaw)
        {
            string frawName = fRaw.Method.Name;
            FRawEnum fRawEnum = frawName switch
            {
                "LinearFunc" => FRawEnum.LinearFunc,
                "ThreePolynomFunc" => FRawEnum.ThreePolynomFunc,
                "RandomValueFunc" => FRawEnum.RandomValueFunc,
                _ => FRawEnum.LinearFunc
            };
            return fRawEnum;
        }
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "RawNumOfNodes":
                        if (RawNumOfNodes < 2)
                        {
                            error = "Число узлов сплайна должно быть больше или равно 2!";
                        }
                        break;
                    case "SplineNumOfNodes":
                        if (SplineNumOfNodes < 2)
                        {
                            error = "Число узлов равномерной сетки для значений сплайна должно быть больше или равно 2!";
                        }
                        break;
                    case "LeftBound":
                        if (LeftBound > RightBound)
                        {
                            error = "Левый конец отрезка интерполяции должен быть меньше, чем правый!";
                        }
                        break;
                    case "RightBound":
                        if (LeftBound > RightBound)
                        {
                            error = "Правый конец отрезка интерполяции должен быть меньше, чем левый!";
                        }
                        break;
                }
                /*if (error != String.Empty)
                    MessageBox.Show(error);*/
                return error;
            }
        }
        public string Error { get; }
    }
}
