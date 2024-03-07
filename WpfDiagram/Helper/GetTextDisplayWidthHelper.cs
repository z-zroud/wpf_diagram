using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace WpfDiagram.Helper
{
    public class GetTextDisplayWidthHelper
    {
        public static Double GetTextDisplayWidth(Label label)
        {

            return GetTextDisplayWidth(label.Content.ToString(), label.FontFamily, label.FontStyle, label.FontWeight, label.FontStretch, label.FontSize);

        }

        public static Double GetTextDisplayWidth(string str, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {

            var formattedText = new FormattedText(
                                str,
                                CultureInfo.CurrentUICulture,
                                FlowDirection.LeftToRight,
                                new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                fontSize,
                                Brushes.Black
                                );

            Size size = new Size(formattedText.Width, formattedText.Height);

            return size.Width;

        }
    }
}
