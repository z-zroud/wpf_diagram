using System.Globalization;

namespace WpfDiagram.Helper
{
    public static class NumberExtensions
    {
        public static string ToInvariantString(this double n) => n.ToString(CultureInfo.InvariantCulture);
    }
}
