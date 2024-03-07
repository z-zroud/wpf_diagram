using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfDiagram.Interface
{
    public interface IShapeViewModel
    {
        ISharpPath SourceMarker
        {
            get; set;
        }

        ISharpPath SinkMarker
        {
            get; set;
        }

        event PropertyChangedEventHandler PropertyChanged;
    }


}
