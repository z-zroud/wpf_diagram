using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using WpfDiagram.Enums;


namespace WpfDiagram.Interface
{
    public interface IColorViewModel
    {
        IColorObject LineColor
        {
            get; set;
        }
        IColorObject FillColor
        {
            get; set;
        }
        Color ShadowColor
        {
            get; set;
        }
        double LineWidth
        {
            get; set;
        }
        LineDashStyle LineDashStyle
        {
            get; set;
        }     
        event PropertyChangedEventHandler PropertyChanged;
    }
}
