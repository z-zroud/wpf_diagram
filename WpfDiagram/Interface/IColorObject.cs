using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfDiagram.Enums;

namespace WpfDiagram.Interface
{
    public interface IColorObject
    {
        BrushType BrushType
        {
            get; set;
        }
        Color Color
        {
            get; set;
        }
        ObservableCollection<GradientStop> GradientStop
        {
            get; set;
        }
        Point StartPoint
        {
            get; set;
        }
        Point EndPoint
        {
            get; set;
        }
        double Opacity
        {
            get; set;
        }
        LinearOrientation LinearOrientation
        {
            get; set;
        }
        RadialOrientation RadialOrientation
        {
            get; set;
        }
        int Angle
        {
            get; set;
        }
        string Image
        {
            get; set;
        }
        int SubType
        {
            get; set;
        }
        Brush ToBrush();
    }
}
