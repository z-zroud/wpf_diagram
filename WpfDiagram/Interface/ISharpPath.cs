using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfDiagram.Enums;

namespace WpfDiagram.Interface
{
    public interface ISharpPath
    {
        string Path
        {
            get; set;
        }

        double Width
        {
            get; set;
        }

        double Height
        {
            get; set;
        }

        PathStyle PathStyle
        {
            get; set;
        }

        SizeStyle SizeStyle
        {
            get; set;
        }
    }
}
