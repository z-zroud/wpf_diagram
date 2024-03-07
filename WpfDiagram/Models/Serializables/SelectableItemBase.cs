using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDiagram.Models.Serializables
{
    public abstract class SelectableItemBase
    {
        public SelectableItemBase()
        {
            ColorItem = new ColorItem() { LineColor = new ColorObjectItem(), FillColor = new ColorObjectItem() };
            FontItem = new FontItem();
            SharpItem = new SharpItem() { SourceMarker = new SharpPathItem(), SinkMarker = new SharpPathItem() };
            AnimationItem = new AnimationItem() { AnimationPath = new SharpPathItem() };
        }

        public SelectableItemBase(SelectableViewModelBase viewmodel)
        {
            this.Id = viewmodel.Id;
            this.ZIndex = viewmodel.ZIndex;
            this.IsGroup = viewmodel.IsGroup;
            this.ParentId = viewmodel.ParentId;
            this.Text = viewmodel.Text;
            this.Name = viewmodel.Name;

            ColorItem = CopyHelper.Mapper<ColorItem>(viewmodel.ColorViewModel);
            FontItem = CopyHelper.Mapper<FontItem>(viewmodel.FontViewModel);
            SharpItem = CopyHelper.Mapper<SharpItem>(viewmodel.ShapeViewModel);
            AnimationItem = CopyHelper.Mapper<AnimationItem>(viewmodel.AnimationViewModel);
        }

        [XmlAttribute]
        public Guid ParentId
        {
            get; set;
        }

        [XmlAttribute]
        public Guid Id
        {
            get; set;
        }

        [XmlAttribute]
        public int ZIndex
        {
            get; set;
        }

        [XmlAttribute]
        public bool IsGroup
        {
            get; set;
        }

        [XmlAttribute]
        public string Name
        {
            get; set;
        }

        [XmlAttribute]
        public string Text
        {
            get; set;
        }

        [XmlElement]
        public ColorItem ColorItem
        {
            get; set;
        }

        [XmlElement]
        public FontItem FontItem
        {
            get; set;
        }

        [XmlElement]
        public SharpItem SharpItem
        {
            get; set;
        }

        [XmlElement]
        public AnimationItem AnimationItem
        {
            get; set;
        }
    }

    [XmlInclude(typeof(ColorItem))]
    public class ColorItem : IColorViewModel
    {
        [XmlIgnore]
        public IColorObject LineColor
        {
            get; set;
        }

        [JsonIgnore]
        [XmlElement("LineColor")]
        public ColorObjectItem XmlLineColor
        {
            get
            {
                return LineColor as ColorObjectItem;
            }
            set
            {
                LineColor = value;
            }
        }

        [XmlIgnore]
        public IColorObject FillColor
        {
            get; set;
        }

        [JsonIgnore]
        [XmlElement("FillColor")]
        public ColorObjectItem XmlFillColor
        {
            get
            {
                return FillColor as ColorObjectItem;
            }
            set
            {
                FillColor = value;
            }
        }


        [XmlIgnore]
        public Color ShadowColor
        {
            get; set;
        }

        [JsonIgnore]
        [XmlElement("ShadowColor")]
        public string XmlShadowColor
        {
            get
            {
                return SerializeHelper.SerializeColor(ShadowColor);
            }
            set
            {
                ShadowColor = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlAttribute]
        public double LineWidth
        {
            get; set;
        }

        [XmlAttribute]
        public LineDashStyle LineDashStyle
        {
            get; set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [XmlInclude(typeof(SharpItem))]
    public class SharpItem : IShapeViewModel
    {
        [XmlIgnore]
        public ISharpPath SourceMarker
        {
            get; set;
        }

        [JsonIgnore]
        [XmlElement("SourceMarker")]
        public SharpPathItem XmlSourceMarker
        {
            get
            {
                return SourceMarker as SharpPathItem;
            }
            set
            {
                SourceMarker = value;
            }
        }

        [XmlIgnore]
        public ISharpPath SinkMarker
        {
            get; set;
        }

        [JsonIgnore]
        [XmlElement("SinkMarker")]
        public SharpPathItem XmlSinkMarker
        {
            get
            {
                return SinkMarker as SharpPathItem;
            }
            set
            {
                SinkMarker = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [XmlInclude(typeof(FontItem))]
    public class FontItem : IFontViewModel
    {
        [XmlIgnore]
        public FontWeight FontWeight
        {
            get; set;
        }
        [XmlIgnore]
        public FontStyle FontStyle
        {
            get; set;
        }
        [XmlIgnore]
        public FontStretch FontStretch
        {
            get; set;
        }
        [XmlAttribute]
        public bool Underline
        {
            get; set;
        }
        [XmlAttribute]
        public bool Strikethrough
        {
            get; set;
        }
        [XmlAttribute]
        public bool OverLine
        {
            get; set;
        }

        [XmlIgnore]
        public Color FontColor
        {
            get; set;
        }

        [JsonIgnore]
        [XmlElement("Color")]
        public string XmlFontColor
        {
            get
            {
                return SerializeHelper.SerializeColor(FontColor);
            }
            set
            {
                FontColor = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlIgnore]
        public string FontFamily
        {
            get; set;
        }

        [XmlIgnore]
        public double FontSize
        {
            get; set;
        }


        [XmlIgnore]
        public System.Drawing.Font FontObject
        {
            get
            {
                var xmlFontStyle = System.Drawing.FontStyle.Regular;
                if (FontStyle == FontStyles.Italic)
                {
                    xmlFontStyle |= System.Drawing.FontStyle.Italic;
                }
                if (FontWeight == FontWeights.Bold)
                {
                    xmlFontStyle |= System.Drawing.FontStyle.Bold;
                }
                return new System.Drawing.Font(FontFamily, (float)FontSize, xmlFontStyle);
            }

            set
            {
                FontFamily = value.FontFamily.Name;
                FontSize = value.Size;
                var xmlFontStyle = value.Style;
                if ((xmlFontStyle & System.Drawing.FontStyle.Italic) == System.Drawing.FontStyle.Italic)
                {
                    FontStyle = FontStyles.Italic;
                }
                else
                {
                    FontStyle = FontStyles.Normal;
                }
                if ((xmlFontStyle & System.Drawing.FontStyle.Bold) == System.Drawing.FontStyle.Bold)
                {
                    FontWeight = FontWeights.Bold;
                }
                else
                {
                    FontWeight = FontWeights.Regular;
                }
            }
        }

        [JsonIgnore]
        [XmlElement("FontObject")]
        public XmlFont XmlFontObject
        {
            get
            {
                return SerializeHelper.SerializeFont(FontObject);
            }

            set
            {
                FontObject = SerializeHelper.DeserializeFont(value);
            }
        }

        [XmlIgnore]
        public Color TextEffectColor
        {
            get; set;
        }

        [JsonIgnore]
        [XmlElement("TextEffectColor")]
        public string XmlTextEffectColor
        {
            get
            {
                return SerializeHelper.SerializeColor(TextEffectColor);
            }
            set
            {
                TextEffectColor = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlIgnore]
        public Color HighlightColor
        {
            get; set;
        }

        [JsonIgnore]
        [XmlElement("HighlightColor")]
        public string XmlHighlightColor
        {
            get
            {
                return SerializeHelper.SerializeColor(HighlightColor);
            }
            set
            {
                HighlightColor = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlAttribute]
        public FontCase FontCase
        {
            get; set;
        }
        [XmlAttribute]
        public HorizontalAlignment HorizontalAlignment
        {
            get; set;
        }
        [XmlAttribute]
        public VerticalAlignment VerticalAlignment
        {
            get; set;
        }
        [XmlAttribute]
        public double LineHeight
        {
            get; set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [XmlInclude(typeof(AnimationItem))]
    public class AnimationItem : IAnimationViewModel
    {
        [XmlAttribute]
        public LineAnimation Animation
        {
            get; set;
        }

        [XmlAttribute]
        public double Duration
        {
            get; set;
        }

        [XmlIgnore]
        public Color Color
        {
            get; set;
        }

        [JsonIgnore]
        [XmlElement("Color")]
        public string XmlColor
        {
            get
            {
                return SerializeHelper.SerializeColor(Color);
            }
            set
            {
                Color = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlIgnore]
        public ISharpPath AnimationPath
        {
            get; set;
        }

        [JsonIgnore]
        [XmlElement("AnimationPath")]
        public SharpPathItem XmlAnimationPath
        {
            get
            {
                return AnimationPath as SharpPathItem;
            }
            set
            {
                AnimationPath = value;
            }
        }

        [XmlAttribute]
        public bool Repeat
        {
            get; set;
        }

        public bool Start
        {
            get; set;
        }

        public int Completed
        {
            get; set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class SerializeHelper
    {
        public static string SerializeColor(Color color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);
        }

        public static Color DeserializeColor(string color)
        {
            byte a, r, g, b;
            try
            {
                if (color?.Length == 9)
                {
                    a = Convert.ToByte(color.Substring(1, 2), 16);
                    r = Convert.ToByte(color.Substring(3, 2), 16);
                    g = Convert.ToByte(color.Substring(5, 2), 16);
                    b = Convert.ToByte(color.Substring(7, 2), 16);
                    return Color.FromArgb(a, r, g, b);
                }
                else if (color?.Length == 7)
                {
                    r = Convert.ToByte(color.Substring(1, 2), 16);
                    g = Convert.ToByte(color.Substring(3, 2), 16);
                    b = Convert.ToByte(color.Substring(5, 2), 16);
                    return Color.FromRgb(r, g, b);
                }
                else
                {
                    return Colors.Black;
                }
            }
            catch
            {
                return Colors.Black;
            }
        }

        public static GradientStop DeserializeGradientStop(string str)
        {
            var strList = str.Split('-');
            return new GradientStop(DeserializeColor(strList[0]), double.Parse(strList[1]));
        }

        public static string SerializeColorList(IEnumerable<Color> colors)
        {
            return string.Join("-", colors.Select(color => SerializeColor(color)));
        }

        public static List<Color> DeserializeColorList(string colorstring)
        {
            List<Color> colorlist = new List<Color>();
            var colors = colorstring.Split('-');
            foreach (var color in colors)
            {
                colorlist.Add(DeserializeColor(color));
            }
            return colorlist;
        }

        public static XmlFont SerializeFont(System.Drawing.Font font)
        {
            return new XmlFont(font);
        }

        public static System.Drawing.Font DeserializeFont(XmlFont font)
        {
            return font.ToFont();
        }

        public static string SerializePoint(Point point)
        {
            return string.Format("{0},{1}", point.X, point.Y);
        }

        public static Point DeserializePoint(string point)
        {
            string[] pieces = point.Split(new char[] { ',' });
            return new Point(double.Parse(pieces[0]), double.Parse(pieces[1]));
        }

        public static string SerializePointList(List<Point> points)
        {
            return string.Join("-", points.Select(point => SerializePoint(point)));
        }

        public static List<Point> DeserializePointList(string pointstring)
        {
            List<Point> pointlist = new List<Point>();
            var points = pointstring.Split('-');
            foreach (var point in points)
            {
                pointlist.Add(DeserializePoint(point));
            }
            return pointlist;
        }

        public static string SerializeSize(Size size)
        {
            return string.Format("{0},{1}", size.Width, size.Height);
        }

        public static Size DeserializeSize(string size)
        {
            string[] pieces = size.Split(new char[] { ',' });
            return new Size(double.Parse(pieces[0]), double.Parse(pieces[1]));
        }

        public static string SerializeCornerRadius(CornerRadius cornerRadius)
        {
            return string.Format("{0},{1},{2},{3}", cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight, cornerRadius.BottomLeft);
        }

        public static CornerRadius DeserializeCornerRadius(string cornerRadius)
        {
            string[] pieces = cornerRadius.Split(new char[] { ',' });
            return new CornerRadius(double.Parse(pieces[0]), double.Parse(pieces[1]), double.Parse(pieces[2]), double.Parse(pieces[3]));
        }

        public static string SerializeThickness(Thickness thickness)
        {
            return string.Format("{0},{1},{2},{3}", thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
        }

        public static Thickness DeserializeThickness(string thickness)
        {
            string[] pieces = thickness.Split(new char[] { ',' });
            return new Thickness(double.Parse(pieces[0]), double.Parse(pieces[1]), double.Parse(pieces[2]), double.Parse(pieces[3]));
        }

        public static string SerializeDoubleNull(double? point)
        {
            return point?.ToString();
        }

        public static double? DeserializeDoubleNull(string point)
        {
            double? value = null;
            if (Double.TryParse(point, out var result))
            {
                value = result;
            }
            return value;
        }

        public static string SerializeMatrix(Matrix matrix)
        {
            return string.Format("{0},{1},{2},{3},{4},{5}", matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.OffsetX, matrix.OffsetY); ;
        }

        public static Matrix DeserializeMatrix(string matrix)
        {
            string[] pieces = matrix.Split(new char[] { ',' });
            return new Matrix(double.Parse(pieces[0]), double.Parse(pieces[1]), double.Parse(pieces[2]), double.Parse(pieces[3]), double.Parse(pieces[4]), double.Parse(pieces[5]));
        }

        public static string SerializeBitmapImage(BitmapImage bitmapImage)
        {
            return bitmapImage.ToBase64String();
        }

        public static BitmapImage DeserializeBitmapImage(string bitmapImagestring)
        {
            try
            {
                return bitmapImagestring.ToBitmapImage();
            }
            catch
            {
                return null;
            }
        }

        public static string SerializeObject(object obj, string serializableType = null)
        {
            if (serializableType?.ToLower() == ".xml")
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, obj);
                    }
                    return textWriter.ToString(); //This is the output as a string
                }
            }
            else
            {
                return JsonConvert.SerializeObject(obj);
            }

        }

        public static SelectableItemBase DeserializeObject(Type type, string serializableString, string serializableType = null)
        {
            if (serializableType?.ToLower() == ".xml")
            {
                using (StringReader sr = new StringReader(serializableString))
                {
                    XmlSerializer serializer = new XmlSerializer(type);
                    return serializer.Deserialize(sr) as SelectableItemBase;
                }
            }
            else
            {
                return JsonConvert.DeserializeObject(serializableString, type) as SelectableItemBase;
            }
        }

        public static SelectableItemBase DeserializeObject(string typename, string serializableString, string serializableType = null)
        {
            Type type = TypeHelper.GetType(typename);
            if (serializableType?.ToLower() == ".xml")
            {
                using (StringReader sr = new StringReader(serializableString))
                {
                    XmlSerializer serializer = new XmlSerializer(type);
                    return serializer.Deserialize(sr) as SelectableItemBase;
                }
            }
            else
            {
                return JsonConvert.DeserializeObject(serializableString, type) as SelectableItemBase;
            }
        }

        public static SelectableItemBase DeserializeObject(SerializableItem serializableItem, string serializableType = null)
        {
            return DeserializeObject(serializableItem.SerializableTypeName, serializableItem.SerializableString, serializableType);
        }
    }

    public struct XmlFont
    {
        public string FontFamily;
        public System.Drawing.GraphicsUnit GraphicsUnit;
        public float Size;
        public System.Drawing.FontStyle Style;

        public XmlFont(System.Drawing.Font f)
        {
            FontFamily = f.FontFamily.Name;
            GraphicsUnit = f.Unit;
            Size = f.Size;
            Style = f.Style;
        }

        public System.Drawing.Font ToFont()
        {
            return new System.Drawing.Font(FontFamily, Size, Style, GraphicsUnit);
        }

    }

    public class ColorObjectItem : IColorObject
    {

        [XmlAttribute]
        public BrushType BrushType
        {
            get; set;
        }

        [XmlIgnore]
        public Color Color
        {
            get; set;
        }

        [JsonIgnore]
        [XmlElement("FillColor")]
        public string XmlFillColor
        {
            get
            {
                return SerializeHelper.SerializeColor(Color);
            }
            set
            {
                Color = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlIgnore]
        public ObservableCollection<GradientStop> GradientStop
        {
            get; set;
        }

        [JsonIgnore]
        [XmlArray("GradientStop")]
        public List<string> XmlGradientStop
        {
            get
            {
                return GradientStop?.Select(p => SerializeHelper.SerializeColor(p.Color) + "-" + p.Offset).ToList();
            }
            set
            {
                GradientStop = new ObservableCollection<GradientStop>(value?.Select(p => SerializeHelper.DeserializeGradientStop(p)));
            }
        }



        [XmlIgnore]
        public IEnumerable<double> Offset
        {
            get; set;
        }

        [JsonIgnore]
        [XmlArray("Offset")]
        public List<double> XmlOffset
        {
            get
            {
                return Offset?.ToList();
            }
            set
            {
                Offset = value;
            }
        }

        [XmlAttribute]
        public string Image
        {
            get; set;
        }

        [XmlAttribute]
        public int SubType
        {
            get; set;
        }

        [XmlIgnore]
        public Point StartPoint
        {
            get; set;
        }

        [JsonIgnore]
        [XmlAttribute("StartPoint")]
        public string XmlStartPoint
        {
            get
            {
                return SerializeHelper.SerializePoint(StartPoint);
            }
            set
            {
                StartPoint = SerializeHelper.DeserializePoint(value);
            }
        }

        [XmlIgnore]
        public Point EndPoint
        {
            get; set;
        }

        [JsonIgnore]
        [XmlAttribute("EndPoint")]
        public string XmlEndPoint
        {
            get
            {
                return SerializeHelper.SerializePoint(EndPoint);
            }
            set
            {
                EndPoint = SerializeHelper.DeserializePoint(value);
            }
        }

        [XmlAttribute]
        public double Opacity
        {
            get; set;
        }
        [XmlAttribute]
        public LinearOrientation LinearOrientation
        {
            get; set;
        }
        [XmlAttribute]
        public RadialOrientation RadialOrientation
        {
            get; set;
        }
        [XmlAttribute]
        public int Angle
        {
            get; set;
        }

        public Brush ToBrush()
        {
            throw new NotImplementedException();
        }
    }

    public class SharpPathItem : ISharpPath
    {
        [XmlAttribute]
        public string Path
        {
            get; set;
        }

        [XmlAttribute]
        public double Width
        {
            get; set;
        }

        [XmlAttribute]
        public double Height
        {
            get; set;
        }

        [XmlAttribute]
        public PathStyle PathStyle
        {
            get; set;
        }

        [XmlAttribute]
        public SizeStyle SizeStyle
        {
            get; set;
        }
    }
}
