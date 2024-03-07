using System;
using System.Collections.Generic;
using WpfDiagram.Attributes;
using WpfDiagram.Common;
using WpfDiagram.Enums;
using WpfDiagram.Interface;

namespace WpfDiagram.ViewModels
{
    public class SharpPath : BindableBase, ISharpPath
    {
        public static SharpPath None { get; } = new SharpPath("", 10, 10, PathStyle.None, SizeStyle.Middle);
        public static SharpPath Arrow { get; } = new SharpPath("M 0 -5 10 0 0 5 z", 10, 10, PathStyle.Arrow, SizeStyle.Middle);
        public static SharpPath Circle { get; } = new SharpPath("M 0, 0 a 5,5 0 1,0 10,0 a 5,5 0 1,0 -10,0", 10, 10, PathStyle.Circle, SizeStyle.Middle);
        public static SharpPath Square { get; } = new SharpPath("M 0 -5 10 -5 10 5 0 5 z", 10, 10, PathStyle.Square, SizeStyle.Middle);
        public static SharpPath Triangle { get; } = new SharpPath("M1,21H23L12,2", 10, 10, PathStyle.Square, SizeStyle.Middle);
        public static SharpPath Rhombus { get; } = new SharpPath("M 0,20 L 30 0 L 60,20 L 30,40 Z", 10, 10, PathStyle.Square, SizeStyle.Middle);
        public static SharpPath Heart { get; } = new SharpPath("M10 3.22l-.61-.6a5.5 5.5 0 0 0-7.78 7.77L10 18.78l8.39-8.4a5.5 5.5 0 0 0-7.78-7.77l-.61.61z", 10, 10, PathStyle.Square, SizeStyle.Middle);
        public static SharpPath Pentagram { get; } = new SharpPath("M 9,2 11,7 17,7 12,10 14,15 9,12 4,15 6,10 1,7 7,7 Z", 10, 10, PathStyle.Square, SizeStyle.Middle);
        public static SharpPath Hexagon { get; } = new SharpPath("M 0,20 L 10,0  H 50 L 60,20 L 50,40 H10 Z", 10, 10, PathStyle.Square, SizeStyle.Middle);

        public static readonly Dictionary<PathStyle, string> SharpPathDictionary = new Dictionary<PathStyle, string>()
        {
            { PathStyle.None, None.Path },
            { PathStyle.Arrow, Arrow.Path },
            { PathStyle.Circle, Circle.Path },
            { PathStyle.Square, Square.Path },
        };

        public SharpPath()
        {

        }

        public SharpPath(string path, double width, double height, PathStyle arrowPathStyle, SizeStyle arrowSizeStyle)
        {
            Path = path;
            Width = width;
            Height = height;
            _pathStyle = arrowPathStyle;
            _sizeStyle = arrowSizeStyle;
        }

        private string _path;
        [CanDo]
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                SetProperty(ref _path, value);
            }
        }

        private double _witdh;
        [CanDo]
        public double Width
        {
            get
            {
                return _witdh;
            }
            set
            {
                SetProperty(ref _witdh, value);
            }
        }

        private double _height;
        [CanDo]
        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                SetProperty(ref _height, value);
            }
        }

        private PathStyle _pathStyle = PathStyle.None;
        [CanDo]
        public PathStyle PathStyle
        {
            get
            {
                return _pathStyle;
            }
            set
            {
                if (SetProperty(ref _pathStyle, value))
                {
                    if (SharpPathDictionary.ContainsKey(_pathStyle))
                    {
                        Path = SharpPathDictionary[_pathStyle];
                    }
                }
            }
        }

        private SizeStyle _sizeStyle = SizeStyle.Middle;
        [CanDo]
        public SizeStyle SizeStyle
        {
            get
            {
                return _sizeStyle;
            }
            set
            {
                if (SetProperty(ref _sizeStyle, value))
                {
                    Width = (double)_sizeStyle;
                    Height = (double)_sizeStyle;
                }
            }
        }

        public static SharpPath NewArrow(double width, double height)
            => new SharpPath(FormattableString.Invariant($"M 0 -{height / 2} {width} 0 0 {height / 2}"), width, height, PathStyle.Arrow, (SizeStyle)width);

        public static SharpPath NewCircle(double r)
            => new SharpPath(FormattableString.Invariant($"M 0, 0 a {r},{r} 0 1,0 {r * 2},0 a {r},{r} 0 1,0 -{r * 2},0"), r * 2, r * 2, PathStyle.Circle, (SizeStyle)(r * 2));

        public static SharpPath NewRectangle(double width, double height)
            => new SharpPath(FormattableString.Invariant($"M 0 -{height / 2} {width} -{height / 2} {width} {height / 2} 0 {height / 2} z"), width, height, PathStyle.Square, (SizeStyle)width);

        public static SharpPath NewSquare(double size) => NewRectangle(size, size);
    }
}
