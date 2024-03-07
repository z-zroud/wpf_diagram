using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDiagram.Interface
{
    public interface IDiagramServiceProvider : INotifyPropertyChanged
    {
        IColorViewModel ColorViewModel
        {
            get;
        }
        IFontViewModel FontViewModel
        {
            get;
        }
        IShapeViewModel ShapeViewModel
        {
            get;
        }
        IAnimationViewModel AnimationViewModel
        {
            get;
        }
        IDrawModeViewModel DrawModeViewModel
        {
            get;
        }
        IQuickThemeViewModel QuickThemeViewModel
        {
            get;
        }
        ILockObjectViewModel LockObjectViewModel
        {
            get;
        }
        SelectableDesignerItemViewModelBase SelectedItemViewModel
        {
            get; set;
        }

        IColorViewModel CopyDefaultColorViewModel();
        IFontViewModel CopyDefaultFontViewModel();
        IShapeViewModel CopyDefaultShapeViewModel();
        IAnimationViewModel CopyDefaultAnimationViewModel();
    }
}
