using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Arc_Progress_Bar.Page.P2
{
    public class P2CircularProgressBar : INotifyPropertyChanged
    {   
        private bool _isLargeArc;
        
        

        private Point _voltage12VStartPoint;
        private Point _voltage12VEndPoint;
        private bool _voltage12VIsLargeArc;

        private Point _efficiencyStartPoint;
        
        private bool _efficiencyIsLargeArc;
        private Point _wattageStartPoint;
        private Point _wattageendPoint;
        private Point _fanStartPoint;
        private Point _fanEndPoint;
        private bool _fanIsLargeArc;

        private Point _tempStartPoint;
        private Point _tempEndPoint;
        private bool _tempIsLargeArc;


       
        private double _wattage;
        private double _fanSpeed;
        private double _voltage12V;
        private double _voltage5V;
        private double _voltage3V3;
        private double _temperature;
        private double _efficiency;
        private double _efficiencyUI;
        private double _temperatureUI;
        private double _wattageUI;

        //mA
        private double _current12V;
        private double _current5V;
        private double _current3V;
        
        private string _progressImage;   //Wattage
        private string _fanprogressImage; //Fan
        private string _tempprogressImage; //Temp
        private string _efficiencyprogressImage; //Efficiency

       
        private Canvas _canvas; // Canvas field
        private Path _bottomArc; // Path field for bottom arc
        private Path _topArc; // Path field for top arc
        private TextBlock _progressText; // TextBlock field for progress text        
        private int _maxValue = 10, _tempmaxValue = 10, _wattagemaxValue = 10, _fanspeedmaxValue = 10;
        private int _progress;
        private int _tempprogress, _wattageprogress, _fanspeedprogress;

        public event PropertyChangedEventHandler PropertyChanged;

        public P2CircularProgressBar(Canvas canvas, Path bottomArc, Path topArc, TextBlock progressText)
        {
            _canvas = canvas;
            _bottomArc = bottomArc;
            _topArc = topArc;
            _progressText = progressText;            
        }        
        
        public P2CircularProgressBar()
        {
            // 初始化屬性
            Voltage12V = 0;
            Voltage5V = 0;
            Voltage3V3 = 0;
            Crueent_12V = 0;
            Crueent_5V = 0;
            Crueent_3V = 0;
        }

        //------ Arc Progress Get Data       
        public double Wattage
        {
            get => _wattage;
            set
            {
                double outWattage = value * 100;
                if (outWattage > 1000.0)
                {
                    outWattage = 1000.0;
                }
                
                _wattage = Math.Round(outWattage, 0);                
                OnPropertyChanged(nameof(Wattage));                
                UpdateWattageArcGeometry();                            
            }
        }
        
        public double WattageUI
        {
            get => _wattageUI;
            set
            {
                _wattageUI = value;
                OnPropertyChanged(nameof(WattageUI));
            }
        }
        public Point StartPoint
        {
            get => _wattageStartPoint;
            set
            {
                _wattageStartPoint = value;
                OnPropertyChanged(nameof(StartPoint));
            }
        }

        public Point EndPoint
        {
            get => _wattageendPoint;
            set
            {
                _wattageendPoint = value;
                OnPropertyChanged(nameof(EndPoint));
            }
        }

        public bool IsLargeArc
        {
            get => _isLargeArc;
            set
            {
                _isLargeArc = value;
                OnPropertyChanged(nameof(IsLargeArc));
            }
        }

        public double FanSpeed
        {
            get => _fanSpeed;
            set
            { 
                double fanSpeedRPM = value * 1000; // Convert to RPM，For example: 0.559 -> 559 RPM
                _fanSpeed = Math.Round(fanSpeedRPM, 0);
                OnPropertyChanged(nameof(FanSpeed));
                UpdateFanSpeedArcGeometry();
            }
        }        

        public bool FanIsLargeArc
        {
            get => _fanIsLargeArc;
            set
            {
                _fanIsLargeArc = value;
                OnPropertyChanged(nameof(FanIsLargeArc));
            }
        }
        
        public double PowerTemp
        {
            get => _temperature;
            set
            {                
                _temperature = Math.Round(value * 100.0, 0);
                if (_temperature > 100) 
                {
                    _temperature = 100; //Under 100 degree
                }
                
                OnPropertyChanged(nameof(PowerTemp));
                UpdateTempProgress();
                UpdateTemperatureArcGeometry();
            }
        }
        
        public double PowerTempUI
        {
            get => _temperatureUI;
            set
            {
                _temperatureUI = value;
                OnPropertyChanged(nameof(PowerTempUI));
            }
        }
        
        public int FanSpeedMaxValue
        {
            get => _fanspeedmaxValue;
            set
            {
                _fanspeedmaxValue = value;
                OnPropertyChanged(nameof(FanSpeedMaxValue));
                UpdateFanSpeedProgress();
                UpdateFanSpeedArcGeometry();
            }
        }
        public int WattageMaxValue
        {
            get => _wattagemaxValue;
            set
            {
                _wattagemaxValue = value;
                OnPropertyChanged(nameof(WattageMaxValue));
                UpdateWattageProgress();
                UpdateWattageArcGeometry();
            }
        }
       
        public int TempMaxValue
        {
            get => _tempmaxValue;
            set
            {                
                _tempmaxValue = value;
                OnPropertyChanged(nameof(TempMaxValue));
                UpdateTempProgress();
                UpdateTemperatureArcGeometry();
            }
        }

        
        public int FanSpeedProgress
        {
            get => _fanspeedprogress;
            set
            {
                _fanspeedprogress = value;
                OnPropertyChanged(nameof(FanSpeedProgress));
                UpdateFanSpeedArcGeometry();
            }
        }

        public int WattageProgress
        {
            get => _wattageprogress;
            set
            {
                _wattageprogress = value;
                OnPropertyChanged(nameof(WattageProgress));
                UpdateWattageArcGeometry();
            }
        }

        public int TempProgress
        {
            get => _tempprogress;
            set
            {
                _tempprogress = value;
                OnPropertyChanged(nameof(TempProgress));
                UpdateTemperatureArcGeometry();
            }
        }

        public double Voltage12V
        {
            get => _voltage12V;
            set
            {
                _voltage12V = Math.Round(value, 2);                
                OnPropertyChanged(nameof(Voltage12V));                
            }
            
        }

        public double Crueent_12V
        {
            get => _current12V;
            set
            {
                _current12V = Math.Round(value, 2);
                OnPropertyChanged(nameof(Crueent_12V));
                UpdateVoltage12VArcGeometry();
            }
        }

        
        public Point Voltage12VStartPoint
        {
            get => _voltage12VStartPoint;
            set
            {
                _voltage12VStartPoint = value;
                OnPropertyChanged(nameof(Voltage12VStartPoint));
            }
        }
        public Point Voltage12VEndPoint
        {
            get => _voltage12VEndPoint;
            set
            {
                _voltage12VEndPoint = value;
                OnPropertyChanged(nameof(Voltage12VEndPoint));
            }
        }

        public bool Voltage12VIsLargeArc
        {
            get => _voltage12VIsLargeArc;
            set
            {
                _voltage12VIsLargeArc = value;
                OnPropertyChanged(nameof(Voltage12VIsLargeArc));
            }
        }

        public double Voltage5V
        {
            get => _voltage5V;
            set
            {
                _voltage5V = Math.Round(value, 2);
                
                OnPropertyChanged(nameof(Voltage5V));                
            }
        }

        public double Crueent_5V
        {
            get => _current5V;
            set
            {
                _current5V = Math.Round(value, 2);
                OnPropertyChanged(nameof(Crueent_5V));
                
            }
        }


        public double Voltage3V3
        {
            get => _voltage3V3;
            set
            {
                _voltage3V3 = Math.Round(value, 3);            
                OnPropertyChanged(nameof(Voltage3V3));
            }
        }

        public double Crueent_3V
        {
            get => _current3V;
            set
            {
                _current3V = Math.Round(value, 2);
                OnPropertyChanged(nameof(Crueent_3V));
            }
        }

        public double Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));                
            }
        }

        public Point TempStartPoint
        {
            get { return _efficiencyStartPoint; }
            set
            {
                _efficiencyStartPoint = value;
                OnPropertyChanged(nameof(TempStartPoint));
            }
        }

        public Point TempEndPoint
        {
            get { return _efficiencyStartPoint; }
            set
            {
                _efficiencyStartPoint = value;
                OnPropertyChanged(nameof(TempEndPoint));
            }
        }

        public bool TempIsLargeArc
        {
            get { return _efficiencyIsLargeArc; }
            set
            {
                _efficiencyIsLargeArc = value;
                OnPropertyChanged(nameof(TempIsLargeArc));
            }
        }
              
        public double Efficiency
        {
            get => _efficiency;
            set
            {
                _efficiency = Math.Round(value * 100.0, 0); // Convert to percentage and round to nearest integer
                
                if(_efficiency > 100)
                {
                    _efficiency = 100;
                }

                OnPropertyChanged(nameof(Efficiency));
                UpdateEfficiencyArcGeometry();
            }
        }

        
        public double EfficiencyUI
        {
            get => _efficiencyUI;
            set
            {
                _efficiencyUI = value * 100.0;
                OnPropertyChanged(nameof(EfficiencyUI));                
            }
        }

        //Render Arc Progress Bar UI
        public int MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                OnPropertyChanged(nameof(MaxValue));
                UpdateProgress();
                UpdateEfficiencyArcGeometry();
            }
        }
        
        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
                UpdateEfficiencyArcGeometry();
            }
        }
        //#FF444442
        static Color bottomSetColor1 = Color.FromArgb(0xff, 0x44, 0x44, 0x42);
        private Brush bottomColor = new SolidColorBrush(bottomSetColor1);
        public Brush BottomColor
        {
            get { return bottomColor; }
            set { bottomColor = value; OnPropertyChanged("BottomColor"); }
        } 

        static Color SetColor1 = Color.FromArgb(0xff, 0xff, 0xcd, 0x00);
        private Brush topColor = new SolidColorBrush(SetColor1);
        public Brush TopColor
        {
            get { return topColor; }
            set { topColor = value; OnPropertyChanged("TopColor"); }
        }

        public Point EfficiencyStartPoint
        {
            get { return _efficiencyStartPoint; }
            set
            {
                _efficiencyStartPoint = value;
                OnPropertyChanged(nameof(EfficiencyStartPoint));                
            }
        }

        public Point EfficiencyEndPoint
        {
            get { return _efficiencyStartPoint; }
            set
            {
                _efficiencyStartPoint = value;
                OnPropertyChanged(nameof(EfficiencyEndPoint));
            }
        }
        
        public bool EfficiencyIsLargeArc
        {
            get { return _efficiencyIsLargeArc; }
            set
            {
                _efficiencyIsLargeArc = value;
                OnPropertyChanged(nameof(EfficiencyIsLargeArc));
            }
        }

        // Arc Geometry properties and methods for each parameter
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Update Wattage Img
        public string ProgressImage
        {
            get => _progressImage;
            private set
            {
                _progressImage = value;
                OnPropertyChanged(nameof(ProgressImage));
            }
        }
        //Update Fan Speed Img
        public string FanProgressImage
        {
            get => _fanprogressImage;
            private set
            {
                _fanprogressImage = value;
                OnPropertyChanged(nameof(FanProgressImage));
            }
        }

        //Update Temperature Img
        public string TempProgressImage
        {
            get => _tempprogressImage;
            private set
            {
                _tempprogressImage = value;
                OnPropertyChanged(nameof(TempProgressImage));
            }
        }

        //Update Efficiency Img
        public string EfficiencyProgressImage
        {
            get => _efficiencyprogressImage;
            private set
            {
                _efficiencyprogressImage = value;
                OnPropertyChanged(nameof(EfficiencyProgressImage));
            }
        }
        

        private void UpdateVoltage12VArcGeometry()
        {
            // Code to update arc geometry for 12V voltage
            double percentage = (_voltage12V - 11) / (12 - 11);
            double angle = percentage * 180;
            double radians = angle * Math.PI / 180;  
            double centerX = 170;
            double centerY = 170;
            double radius = 96;
            double endX = centerX + radius * Math.Cos(radians);
            double endY = centerY - radius * Math.Sin(radians);
            
            Voltage12VStartPoint = new Point(centerX + radius, centerY);            
            Voltage12VEndPoint = new Point(endX, endY);            
            Voltage12VIsLargeArc = angle > 180;            

        }     

        //Update Arc UI of FanSpeed data
        private void UpdateFanSpeedArcGeometry() 
        {
            double size = Math.Min(_canvas.ActualWidth, _canvas.ActualHeight);
            if (size <= 0) return;

            double centerX = _canvas.ActualWidth / 2;
            double centerY = _canvas.ActualHeight / 2;
            double radius = size / 2 - 20; // Adjust for thickness

            double startAngle = 140;
            double startX = centerX + radius * Math.Cos(startAngle * Math.PI / 180);
            double startY = centerY + radius * Math.Sin(startAngle * Math.PI / 180);

            PathGeometry bottomGeometry = new PathGeometry();
            PathFigure bottomFigure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                IsClosed = false
            };
            ArcSegment bottomArcSegment = new ArcSegment
            {
                Point = new Point(centerX + radius * Math.Cos((startAngle + 260) * Math.PI / 180),
                                  centerY + radius * Math.Sin((startAngle + 260) * Math.PI / 180)),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = true
            };
            bottomFigure.Segments.Add(bottomArcSegment);
            bottomGeometry.Figures.Add(bottomFigure);
            _bottomArc.Data = bottomGeometry;

            // Calculate the progress angle based on percentage
            double progressAngle = 260 * ((_fanSpeed / 100.0) / FanSpeedMaxValue); 
            double endAngle = startAngle + progressAngle;
            double endX = centerX + radius * Math.Cos(endAngle * Math.PI / 180);
            double endY = centerY + radius * Math.Sin(endAngle * Math.PI / 180);

            PathGeometry topGeometry = new PathGeometry();
            PathFigure topFigure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                IsClosed = false
            };
            ArcSegment topArcSegment = new ArcSegment
            {
                Point = new Point(endX, endY),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = progressAngle > 180
            };
            topFigure.Segments.Add(topArcSegment);
            topGeometry.Figures.Add(topFigure);
            _topArc.Data = topGeometry;

            Canvas.SetLeft(_progressText, centerX - _progressText.ActualWidth / 2);
            Canvas.SetTop(_progressText, centerY - _progressText.ActualHeight / 2);

            
            double fanSpeedPercentage = _fanSpeed; // RPM Convert to %
            _progressText.Text = $"{fanSpeedPercentage}"; // Show RPM
        }
        private void UpdateWattageArcGeometry()
        {
            // Code to update arc geometry for Wattage
            double size = Math.Min(_canvas.ActualWidth, _canvas.ActualHeight);
            if (size <= 0) return;

            double centerX = _canvas.ActualWidth / 2;
            double centerY = _canvas.ActualHeight / 2;
            double radius = size / 2 - 20; // Adjust for thickness

            double startAngle = 140;
            double startX = centerX + radius * Math.Cos(startAngle * Math.PI / 180);
            double startY = centerY + radius * Math.Sin(startAngle * Math.PI / 180);

            PathGeometry bottomGeometry = new PathGeometry();
            PathFigure bottomFigure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                IsClosed = false
            };
            ArcSegment bottomArcSegment = new ArcSegment
            {
                Point = new Point(centerX + radius * Math.Cos((startAngle + 260) * Math.PI / 180),
                                  centerY + radius * Math.Sin((startAngle + 260) * Math.PI / 180)),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = true
            };
            bottomFigure.Segments.Add(bottomArcSegment);
            bottomGeometry.Figures.Add(bottomFigure);
            _bottomArc.Data = bottomGeometry;

            //double progressAngle = 260 * (WattageProgress / (double)WattageMaxValue);
            double progressAngle = 260 * ((_wattage / 100.0) / WattageMaxValue);
            double endAngle = startAngle + progressAngle;
            double endX = centerX + radius * Math.Cos(endAngle * Math.PI / 180);
            double endY = centerY + radius * Math.Sin(endAngle * Math.PI / 180);

            PathGeometry topGeometry = new PathGeometry();
            PathFigure topFigure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                IsClosed = false
            };
            ArcSegment topArcSegment = new ArcSegment
            {
                Point = new Point(endX, endY),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = progressAngle > 180
            };
            topFigure.Segments.Add(topArcSegment);
            topGeometry.Figures.Add(topFigure);
            _topArc.Data = topGeometry;

            Canvas.SetLeft(_progressText, centerX - _progressText.ActualWidth / 2);
            Canvas.SetTop(_progressText, centerY - _progressText.ActualHeight / 2);
            
            double wattagePercentage = _wattage;
            _progressText.Text = $"{wattagePercentage}";            
        }
        private void UpdateTemperatureArcGeometry()
        {
            // Code to update arc geometry for Temperature
            double size = Math.Min(_canvas.ActualWidth, _canvas.ActualHeight);
            if (size <= 0) return;

            double centerX = _canvas.ActualWidth / 2;
            double centerY = _canvas.ActualHeight / 2;
            double radius = size / 2 - 20; // Adjust for thickness

            double startAngle = 140;
            double startX = centerX + radius * Math.Cos(startAngle * Math.PI / 180);
            double startY = centerY + radius * Math.Sin(startAngle * Math.PI / 180);

            PathGeometry bottomGeometry = new PathGeometry();
            PathFigure bottomFigure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                IsClosed = false
            };
            ArcSegment bottomArcSegment = new ArcSegment
            {
                Point = new Point(centerX + radius * Math.Cos((startAngle + 260) * Math.PI / 180),
                                  centerY + radius * Math.Sin((startAngle + 260) * Math.PI / 180)),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = true
            };
            bottomFigure.Segments.Add(bottomArcSegment);
            bottomGeometry.Figures.Add(bottomFigure);
            _bottomArc.Data = bottomGeometry;

            double progressAngle = 260 * (_temperature / 100.0);
            double endAngle = startAngle + progressAngle;
            double endX = centerX + radius * Math.Cos(endAngle * Math.PI / 180);
            double endY = centerY + radius * Math.Sin(endAngle * Math.PI / 180);

            PathGeometry topGeometry = new PathGeometry();
            PathFigure topFigure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                IsClosed = false
            };
            ArcSegment topArcSegment = new ArcSegment
            {
                Point = new Point(endX, endY),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = progressAngle > 180
            };
            topFigure.Segments.Add(topArcSegment);
            topGeometry.Figures.Add(topFigure);
            _topArc.Data = topGeometry;

            Canvas.SetLeft(_progressText, centerX - _progressText.ActualWidth / 2);
            Canvas.SetTop(_progressText, centerY - _progressText.ActualHeight / 2);
            _progressText.Text = $"{(int)_temperature}";
        }
        
        private void UpdateFanSpeedProgress()
        {
            // According Fan Speed value update Progress            
            FanSpeedProgress = (int)(_fanSpeed / FanSpeedMaxValue * 100); // Let Fan Speed convert to percentage value, FanSpeed range is 0 to 1

        }
        private void UpdateWattageProgress()
        {
            // According Wattage value update Progress
            WattageProgress = (int)(_wattage * WattageMaxValue); // Wattage range is 0 to 1            
        }
        private void UpdateTempProgress()
        {
            // According Tempature value update Progress            
            TempProgress = (int)(_temperature / TempMaxValue * 100); // temperature percentage, Tempature range is 0 to 1

        }
        //Add this method for Efficiency Arc Progress bar 24/05/29
        private void UpdateProgress()
        {
            // According Efficiency value update Progress
            Progress = (int)(_efficiency * MaxValue); // Efficiency range is 0 to 1
        }
        private void UpdateEfficiencyArcGeometry()
        {
            // Code to update arc geometry for efficiency
            double size = Math.Min(_canvas.ActualWidth, _canvas.ActualHeight);
            if (size <= 0) return;

            double centerX = _canvas.ActualWidth / 2;
            double centerY = _canvas.ActualHeight / 2;
            double radius = size / 2 - 20; // Adjust for thickness

            double startAngle = 140;
            double startX = centerX + radius * Math.Cos(startAngle * Math.PI / 180);
            double startY = centerY + radius * Math.Sin(startAngle * Math.PI / 180);

            PathGeometry bottomGeometry = new PathGeometry();
            PathFigure bottomFigure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                IsClosed = false
            };
            ArcSegment bottomArcSegment = new ArcSegment
            {
                Point = new Point(centerX + radius * Math.Cos((startAngle + 260) * Math.PI / 180),
                                  centerY + radius * Math.Sin((startAngle + 260) * Math.PI / 180)),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = true
            };
            bottomFigure.Segments.Add(bottomArcSegment);
            bottomGeometry.Figures.Add(bottomFigure);
            _bottomArc.Data = bottomGeometry;            

            double progressAngle = 260 * (Efficiency / 100.0);
            double endAngle = startAngle + progressAngle;
            double endX = centerX + radius * Math.Cos(endAngle * Math.PI / 180);
            double endY = centerY + radius * Math.Sin(endAngle * Math.PI / 180);

            PathGeometry topGeometry = new PathGeometry();
            PathFigure topFigure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                IsClosed = false
            };
            ArcSegment topArcSegment = new ArcSegment
            {
                Point = new Point(endX, endY),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = progressAngle > 180
            };
            topFigure.Segments.Add(topArcSegment);
            topGeometry.Figures.Add(topFigure);
            _topArc.Data = topGeometry;

            Canvas.SetLeft(_progressText, centerX - _progressText.ActualWidth / 2);
            Canvas.SetTop(_progressText, centerY - _progressText.ActualHeight / 2);            
            _progressText.Text = $"{Efficiency}";
        }
    }
}
