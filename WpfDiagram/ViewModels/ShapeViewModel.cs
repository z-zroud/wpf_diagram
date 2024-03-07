using System;
using System.Collections.Generic;
using System.Text;
using WpfDiagram.Common;
using WpfDiagram.Interface;

namespace WpfDiagram.ViewModels
{
    public class ShapeViewModel : BindableBase, IShapeViewModel
    {
        private ISharpPath _sourceMarker = SharpPath.None;
        public ISharpPath SourceMarker
        {
            get
            {
                return _sourceMarker;
            }
            set
            {
                if (_sourceMarker != value)
                {
                    if (_sourceMarker != null && _sourceMarker is SharpPath _linkMarker1)
                    {
                        _linkMarker1.PropertyChanged -= ShapeViewModel_PropertyChanged;
                    }
                    SetProperty(ref _sourceMarker, value);
                    if (_sourceMarker != null && _sourceMarker is SharpPath _linkMarker2)
                    {
                        _linkMarker2.PropertyChanged += ShapeViewModel_PropertyChanged;
                    }
                }
                else
                {
                    RaisePropertyChanged(nameof(SourceMarker));
                }
            }
        }

        private ISharpPath _sinkMarker = SharpPath.Arrow;
        public ISharpPath SinkMarker
        {
            get
            {
                return _sinkMarker;
            }
            set
            {
                if (_sinkMarker != value)
                {
                    if (_sinkMarker != null && _sinkMarker is SharpPath _linkMarker1)
                    {
                        _linkMarker1.PropertyChanged -= ShapeViewModel_PropertyChanged;
                    }
                    SetProperty(ref _sinkMarker, value);
                    if (_sinkMarker != null && _sinkMarker is SharpPath _linkMarker2)
                    {
                        _linkMarker2.PropertyChanged += ShapeViewModel_PropertyChanged;
                    }
                }
                else
                {
                    RaisePropertyChanged(nameof(SinkMarker));
                }
            }
        }

        private void ShapeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender == SourceMarker)
            {
                RaisePropertyChanged(nameof(SourceMarker));
            }
            else if (sender == SinkMarker)
            {
                RaisePropertyChanged(nameof(SinkMarker));
            }
        }
    }

  

    
}
