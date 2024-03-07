using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfDiagram.Attributes;
using WpfDiagram.Common;
using WpfDiagram.Enums;
using WpfDiagram.Interface;

namespace WpfDiagram.ViewModels
{
    [Serializable]
    public class AnimationViewModel : BindableBase, IAnimationViewModel
    {
        private LineAnimation _animation = LineAnimation.None;
        [CanDo]
        public LineAnimation Animation
        {
            get
            {
                return _animation;
            }
            set
            {
                SetProperty(ref _animation, value);
            }
        }

        private double _duration = 1;
        [CanDo]
        public double Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                SetProperty(ref _duration, value);
            }
        }

        private Color _color = Colors.Red;
        [CanDo]
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                if (!SetProperty(ref _color, value))
                {
                    RaisePropertyChanged(nameof(Color));
                }
            }
        }

        private ISharpPath _animationPath = SharpPath.Circle;
        public ISharpPath AnimationPath
        {
            get
            {
                return _animationPath;
            }
            set
            {
                if (_animationPath != value)
                {
                    if (_animationPath != null && _animationPath is SharpPath _sharpPath1)
                    {
                        _sharpPath1.PropertyChanged -= AnimationViewModel_PropertyChanged;
                    }
                    SetProperty(ref _animationPath, value);
                    if (_animationPath != null && _animationPath is SharpPath _sharpPath2)
                    {
                        _sharpPath2.PropertyChanged += AnimationViewModel_PropertyChanged;
                    }
                }
                else
                {
                    RaisePropertyChanged(nameof(AnimationPath));
                }
            }
        }

        private bool _repeat = true;
        public bool Repeat
        {
            get
            {
                return _repeat;
            }
            set
            {
                SetProperty(ref _repeat, value);
            }
        }

        private bool _start;
        public bool Start
        {
            get
            {
                return _start;
            }
            set
            {
                SetProperty(ref _start, value);
            }
        }

        public int Completed
        {
            get; set;
        }

        private void AnimationViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender == AnimationPath)
            {
                RaisePropertyChanged(nameof(AnimationPath));
            }
        }
    }

}
