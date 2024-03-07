using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfDiagram.Enums;

namespace WpfDiagram.Interface
{
    public interface IAnimationViewModel
    {
        LineAnimation Animation
        {
            get; set;
        }
        double Duration
        {
            get; set;
        }
        Color Color
        {
            get; set;
        }
        ISharpPath AnimationPath
        {
            get; set;
        }

        bool Repeat
        {
            get; set;
        }

        bool Start
        {
            get; set;
        }

        int Completed
        {
            get; set;
        }
        event PropertyChangedEventHandler PropertyChanged;
    }
}
