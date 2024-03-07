using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using WpfDiagram.Common;
using WpfDiagram.Enums;
using WpfDiagram.Interface;
using WpfDiagram.Attributes;

namespace WpfDiagram.ViewModels
{
    [Serializable]
    public class ColorViewModel : BindableBase, IColorViewModel
    {
        #region 界面使用
        public static Color[] FillColors { get; } = new Color[] { Colors.Red, Colors.Green, Colors.Blue, Colors.White, Colors.Black, Colors.Purple };
        public static Color[] LineColors { get; } = new Color[] { Colors.Red, Colors.Green, Colors.Blue, Colors.White, Colors.Black, Colors.Purple };
        #endregion

        public ColorViewModel(Color linecolor, Color fillcolor)
        {
            LineColor = new ColorObject() { Color = linecolor };
            FillColor = new ColorObject() { Color = fillcolor };
        }

        public ColorViewModel()
        {
            LineColor = new ColorObject() { Color = Colors.Gray };
            FillColor = new ColorObject() { Color = Colors.White };
        }
        private IColorObject _lineColor;
        public IColorObject LineColor
        {
            get
            {
                return _lineColor;
            }
            set
            {
                if (_lineColor != value)
                {
                    if (_lineColor != null && _lineColor is ColorObject _lineColor1)
                    {
                        _lineColor1.PropertyChanged -= ColorViewModel_PropertyChanged;
                    }
                    SetProperty(ref _lineColor, value);
                    if (_lineColor != null && _lineColor is ColorObject _lineColor2)
                    {
                        _lineColor2.PropertyChanged += ColorViewModel_PropertyChanged;
                    }
                }
                else
                {
                    RaisePropertyChanged(nameof(LineColor));
                }
            }
        }

        private IColorObject _fillcolor;
        public IColorObject FillColor
        {
            get
            {
                return _fillcolor;
            }
            set
            {
                if (_fillcolor != value)
                {
                    if (_fillcolor != null && _fillcolor is ColorObject colorObject1)
                    {
                        colorObject1.PropertyChanged -= ColorViewModel_PropertyChanged;
                    }
                    SetProperty(ref _fillcolor, value);
                    if (_fillcolor != null && _fillcolor is ColorObject colorObject2)
                    {
                        colorObject2.PropertyChanged += ColorViewModel_PropertyChanged;
                    }
                }
                else
                {
                    RaisePropertyChanged(nameof(FillColor));
                }
            }
        }

        private void ColorViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender == LineColor)
            {
                RaisePropertyChanged(nameof(LineColor));
            }
            else if (sender == FillColor)
            {
                RaisePropertyChanged(nameof(FillColor));
            }

            RaisePropertyChanged(sender, e);
        }

        private Color _shadowColor = Colors.Transparent;
        [CanDo]
        public Color ShadowColor
        {
            get
            {
                return _shadowColor;
            }
            set
            {
                if (!SetProperty(ref _shadowColor, value))
                {
                    RaisePropertyChanged(nameof(ShadowColor));
                }
            }
        }

        private double _lineWidth = 1d;
        [CanDo]
        public double LineWidth
        {
            get
            {
                return _lineWidth;
            }
            set
            {
                if (!SetProperty(ref _lineWidth, value))
                {
                    RaisePropertyChanged(nameof(LineWidth));
                }
            }
        }

        private LineDashStyle _lineDashStyle = LineDashStyle.None;
        [CanDo]
        public LineDashStyle LineDashStyle
        {
            get
            {
                return _lineDashStyle;
            }
            set
            {
                if (!SetProperty(ref _lineDashStyle, value))
                {
                    RaisePropertyChanged(nameof(LineDashStyle));
                }
            }
        }
    }


    [Serializable]
    public class ColorObject : BindableBase, IColorObject
    {
        public ColorObject()
        {

        }

        private void GradientStop_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var old in e.OldItems.OfType<GradientStop>())
                {
                    old.PropertyChanged -= GradientStop_PropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (var old in e.NewItems.OfType<GradientStop>())
                {
                    old.PropertyChanged += GradientStop_PropertyChanged;
                }
            }
        }

        private void GradientStop_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(GradientStop));
        }


        public void BrushTypeChanged()
        {
            if (BrushType == BrushType.LinearGradientBrush || BrushType == BrushType.RadialGradientBrush)
            {
                if (GradientStop == null)
                {
                    GradientStop = new ObservableCollection<GradientStop>();
                    GradientStop.Add(new GradientStop(Color, 0));
                    GradientStop.Add(new GradientStop(Colors.Gray, 1));
                    SelectedGradientStop = GradientStop.FirstOrDefault();
                    RaisePropertyChanged(nameof(GradientStop));
                }
            }
        }

        private BrushType _brushType = BrushType.SolidColorBrush;
        [CanDo]
        public BrushType BrushType
        {
            get
            {
                return _brushType;
            }
            set
            {
                if (SetProperty(ref _brushType, value))
                {
                    BrushTypeChanged();
                }
            }
        }

        private Color _color = new Color();
        [CanDo]
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                SetProperty(ref _color, value);
            }
        }

        private ObservableCollection<GradientStop> _gradientStop;
        public ObservableCollection<GradientStop> GradientStop
        {
            get
            {
                return _gradientStop;
            }
            set
            {
                if (_gradientStop != value)
                {
                    if (_gradientStop != null)
                    {
                        _gradientStop.CollectionChanged -= GradientStop_CollectionChanged;
                    }
                    SetProperty(ref _gradientStop, value);
                    if (_gradientStop != null)
                    {
                        _gradientStop.CollectionChanged += GradientStop_CollectionChanged;
                    }
                }
            }
        }

        private GradientStop _selectedGradientStop;
        public GradientStop SelectedGradientStop
        {
            get
            {
                return _selectedGradientStop;
            }
            set
            {
                SetProperty(ref _selectedGradientStop, value);
            }
        }

        private Point _startPoint;
        public Point StartPoint
        {
            get
            {
                return _startPoint;
            }
            set
            {
                SetProperty(ref _startPoint, value);
            }
        }

        private Point _endPoint;
        public Point EndPoint
        {
            get
            {
                return _endPoint;
            }
            set
            {
                SetProperty(ref _endPoint, value);
            }
        }

        private double _opacity = 1;
        [CanDo]
        public double Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                SetProperty(ref _opacity, value);
            }
        }

        private double _light;
        public double Light
        {
            get
            {
                return _light;
            }
            set
            {
                SetProperty(ref _light, value);
            }
        }

        private string _image;
        [CanDo]
        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                SetProperty(ref _image, value);
            }
        }

        private LinearOrientation _linearOrientation;
        [CanDo]
        public LinearOrientation LinearOrientation
        {
            get
            {
                return _linearOrientation;
            }
            set
            {
                SetProperty(ref _linearOrientation, value);
            }
        }

        private RadialOrientation _radialOrientation;
        [CanDo]
        public RadialOrientation RadialOrientation
        {
            get
            {
                return _radialOrientation;
            }
            set
            {
                SetProperty(ref _radialOrientation, value);
            }
        }

        private int _angle;
        [CanDo]
        public int Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                SetProperty(ref _angle, value);
            }
        }

        private int _subType;

        public int SubType
        {
            get
            {
                return _subType;
            }
            set
            {
                SetProperty(ref _subType, value);
            }
        }

        public ICommand AddGradientStopCommand
        {
            get
            {
                return new SimpleCommand(para => {
                    var offset = GradientStop.Skip(GradientStop.Count - 2).Select(p => p.Offset).Average();
                    GradientStop.Add(new GradientStop(Colors.Gray, offset));
                });
            }
        }
        public ICommand DeleteGradientStopCommand
        {
            get
            {
                return new SimpleCommand(para => {
                    if (SelectedGradientStop != null && GradientStop != null && GradientStop.Count > 2)
                    {
                        GradientStop.Remove(SelectedGradientStop);
                    }
                });
            }
        }

        public Brush ToBrush()
        {
            Brush brush = null;

            if (BrushType == BrushType.None)
                brush = new SolidColorBrush(Colors.Transparent);
            else if (BrushType == BrushType.SolidColorBrush)
                brush = new SolidColorBrush(Color);
            else if (BrushType == BrushType.LinearGradientBrush)
            {
                Point startPoint;
                Point endPoint;
                if (LinearOrientation == LinearOrientation.LeftToRight)
                {
                    startPoint = new Point(0, 0.5);
                    endPoint = new Point(1, 0.5);
                }
                else if (LinearOrientation == LinearOrientation.LeftTopToRightBottom)
                {
                    startPoint = new Point(0, 0);
                    endPoint = new Point(1, 1);
                }
                else if (LinearOrientation == LinearOrientation.TopToBottom)
                {
                    startPoint = new Point(0.5, 0);
                    endPoint = new Point(0.5, 1);
                }
                else if (LinearOrientation == LinearOrientation.RightTopToLeftBottom)
                {
                    startPoint = new Point(1, 0);
                    endPoint = new Point(0, 1);
                }
                else if (LinearOrientation == LinearOrientation.RightToLeft)
                {
                    startPoint = new Point(1, 0.5);
                    endPoint = new Point(0, 0.5);
                }
                else if (LinearOrientation == LinearOrientation.RightBottomToLeftTop)
                {
                    startPoint = new Point(1, 1);
                    endPoint = new Point(0, 0);
                }
                else if (LinearOrientation == LinearOrientation.BottomToTop)
                {
                    startPoint = new Point(0.5, 1);
                    endPoint = new Point(0.5, 0);
                }
                else if (LinearOrientation == LinearOrientation.LeftBottomToRightTop)
                {
                    startPoint = new Point(0, 1);
                    endPoint = new Point(1, 0);
                }
                else
                {
                    startPoint = new Point(0, 0.5);
                    endPoint = new Point(1, 0.5);
                }

                LinearGradientBrush myBrush = new LinearGradientBrush();
                myBrush.StartPoint = startPoint;
                myBrush.EndPoint = endPoint;
                if (GradientStop != null)
                {
                    foreach (var stop in GradientStop)
                    {
                        myBrush.GradientStops.Add(new System.Windows.Media.GradientStop(stop.Color, stop.Offset));
                    }
                }
                brush = myBrush;

                RotateTransform rotateTransform = new RotateTransform(Angle, 0.5, 0.5);
                myBrush.RelativeTransform = rotateTransform;
            }
            else if (BrushType == BrushType.RadialGradientBrush)
            {
                Point center;
                Point gradientOrigin;
                double radiusX;
                double radiusY;

                if (RadialOrientation == RadialOrientation.LeftTop)
                {
                    center = new Point(0, 0);
                    gradientOrigin = center;
                    radiusX = 1;
                    radiusY = 1;
                }
                else if (RadialOrientation == RadialOrientation.RightTop)
                {
                    center = new Point(1, 0);
                    gradientOrigin = center;
                    radiusX = 1;
                    radiusY = 1;
                }
                else if (RadialOrientation == RadialOrientation.RightBottom)
                {
                    center = new Point(1, 1);
                    gradientOrigin = center;
                    radiusX = 1;
                    radiusY = 1;
                }
                else if (RadialOrientation == RadialOrientation.LeftBottom)
                {
                    center = new Point(0, 1);
                    gradientOrigin = center;
                    radiusX = 1;
                    radiusY = 1;
                }
                else
                {
                    center = new Point(0.5, 0.5);
                    gradientOrigin = center;
                    radiusX = 0.5;
                    radiusY = 0.5;
                }

                RadialGradientBrush myBrush = new RadialGradientBrush();
                myBrush.Center = center;
                myBrush.GradientOrigin = gradientOrigin;
                myBrush.RadiusX = radiusX;
                myBrush.RadiusY = radiusY;
                if (GradientStop != null)
                {
                    foreach (var stop in GradientStop)
                    {
                        myBrush.GradientStops.Add(new System.Windows.Media.GradientStop(stop.Color, stop.Offset));
                    }
                }
                brush = myBrush;

                RotateTransform rotateTransform = new RotateTransform(Angle, 0.5, 0.5);
                myBrush.RelativeTransform = rotateTransform;
            }
            else if (BrushType == BrushType.ImageBrush)
            {
                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri(Image, UriKind.Absolute));
                brush = myBrush;
            }
            else if (BrushType == BrushType.DrawingBrush)
            {
                DrawingBrush myBrush = new DrawingBrush();

                GeometryDrawing backgroundSquare =
                    new GeometryDrawing(
                        Brushes.White,
                        null,
                        new RectangleGeometry(new Rect(0, 0, 100, 100)));

                GeometryGroup aGeometryGroup = new GeometryGroup();
                aGeometryGroup.Children.Add(new RectangleGeometry(new Rect(0, 0, 50, 50)));
                aGeometryGroup.Children.Add(new RectangleGeometry(new Rect(50, 50, 50, 50)));

                LinearGradientBrush checkerBrush = new LinearGradientBrush();
                checkerBrush.GradientStops.Add(new System.Windows.Media.GradientStop(Colors.Black, 0.0));
                checkerBrush.GradientStops.Add(new System.Windows.Media.GradientStop(Colors.Gray, 1.0));

                GeometryDrawing checkers = new GeometryDrawing(checkerBrush, null, aGeometryGroup);

                DrawingGroup checkersDrawingGroup = new DrawingGroup();
                checkersDrawingGroup.Children.Add(backgroundSquare);
                checkersDrawingGroup.Children.Add(checkers);

                myBrush.Drawing = checkersDrawingGroup;
                myBrush.Viewport = new Rect(0, 0, 0.25, 0.25);
                myBrush.TileMode = TileMode.Tile;

                brush = myBrush;
            }
            if (brush != null)
            {
                brush.Opacity = Opacity;
            }

            return brush;
        }
    }


    public class GradientStop : BindableBase
    {
        public GradientStop()
        {

        }
        public GradientStop(Color color, double offset)
        {
            Color = color;
            Offset = offset;
        }
        private Color _color = new Color();
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                SetProperty(ref _color, value);
            }
        }

        private double _offset;
        public double Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                SetProperty(ref _offset, value);
            }
        }



    }
}
