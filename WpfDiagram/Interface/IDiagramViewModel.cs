using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfDiagram.Interface
{
    public interface IDiagramViewModel
    {
        string Name
        {
            get; set;
        }
        List<SelectableDesignerItemViewModelBase> SelectedItems
        {
            get;
        }
        SelectableDesignerItemViewModelBase SelectedItem
        {
            get;
        }
        ObservableCollection<SelectableDesignerItemViewModelBase> Items
        {
            get;
        }
        DiagramSelectionService SelectionService
        {
            get;
        }

        ICommand ClearCommand
        {
            get;
        }
        ICommand ClearSelectedItemsCommand
        {
            get;
        }
        ICommand AlignTopCommand
        {
            get;
        }
        ICommand AlignVerticalCentersCommand
        {
            get;
        }
        ICommand AlignBottomCommand
        {
            get;
        }
        ICommand AlignLeftCommand
        {
            get;
        }
        ICommand AlignHorizontalCentersCommand
        {
            get;
        }
        ICommand AlignRightCommand
        {
            get;
        }
        ICommand BringForwardCommand
        {
            get;
        }
        ICommand BringToFrontCommand
        {
            get;
        }
        ICommand SendBackwardCommand
        {
            get;
        }
        ICommand SendToBackCommand
        {
            get;
        }

        ICommand DistributeHorizontalCommand
        {
            get;
        }
        ICommand DistributeVerticalCommand
        {
            get;
        }
        ICommand SelectAllCommand
        {
            get;
        }
        ICommand SelectInverseCommand
        {
            get;
        }
        ICommand SelectItemCommand
        {
            get;
        }
        ICommand AddCommand
        {
            get;
        }
        ICommand DeleteCommand
        {
            get;
        }
        ICommand CopyCommand
        {
            get;
        }
        ICommand PasteCommand
        {
            get;
        }
        ICommand CutCommand
        {
            get;
        }
        ICommand LeftMoveCommand
        {
            get;
        }
        ICommand RightMoveCommand
        {
            get;
        }
        ICommand UpMoveCommand
        {
            get;
        }
        ICommand DownMoveCommand
        {
            get;
        }
        ICommand CenterMoveCommand
        {
            get;
        }
        ICommand SameSizeCommand
        {
            get;
        }
        ICommand SameWidthCommand
        {
            get;
        }
        ICommand SameHeightCommand
        {
            get;
        }
        ICommand SameAngleCommand
        {
            get;
        }
        ICommand FitAutoCommand
        {
            get;
        }
        ICommand FitWidthCommand
        {
            get;
        }
        ICommand FitHeightCommand
        {
            get;
        }
        ICommand GroupCommand
        {
            get;
        }
        ICommand UngroupCommand
        {
            get;
        }
        ICommand LockCommand
        {
            get;
        }
        ICommand UnlockCommand
        {
            get;
        }
        ICommand EditCommand
        {
            get;
        }
        ICommand UndoCommand
        {
            get;
        }
        ICommand RedoCommand
        {
            get;
        }
        ICommand ResetLayoutCommand
        {
            get;
        }
        ICommand ShowSearchCommand
        {
            get;
        }
        ICommand CloseSearchCommand
        {
            get;
        }
        ICommand SearchDownCommand
        {
            get;
        }
        ICommand SearchUpCommand
        {
            get;
        }
        ICommand ReplaceCommand
        {
            get;
        }
        ICommand ReplaceAllCommand
        {
            get;
        }

        DiagramType DiagramType
        {
            get; set;
        }

        event DiagramEventHandler Event;

        bool IsReadOnly
        {
            get; set;
        }
        bool IsLoading
        {
            get; set;
        }
        double ZoomValue
        {
            get; set;
        }
        double MaximumZoomValue
        {
            get; set;
        }
        double MinimumZoomValue
        {
            get; set;
        }
        bool DefaultZoomBox
        {
            get; set;
        }
        System.Windows.Point CurrentPoint
        {
            get; set;
        }
        Color CurrentColor
        {
            get; set;
        }
        bool ShowSearch
        {
            get; set;
        }
        string SearchText
        {
            get; set;
        }
        string ReplaceText
        {
            get; set;
        }
        string SearchInfo
        {
            get; set;
        }
        bool SearchCaseMatch
        {
            get; set;
        }
        bool SearchWholeWordMatch
        {
            get; set;
        }
        Brush Thumbnail
        {
            get; set;
        }
        #region 如果这个赋值了，优先用这个的
        IDrawModeViewModel DrawModeViewModel
        {
            get; set;
        }
        IColorViewModel ColorViewModel
        {
            get; set;
        }
        IFontViewModel FontViewModel
        {
            get; set;
        }
        IShapeViewModel ShapeViewModel
        {
            get; set;
        }
        IAnimationViewModel AnimationViewModel
        {
            get; set;
        }
        #endregion

        DoCommandManager DoCommandManager
        {
            get;
        }
        #region 设置选项
        DiagramOption DiagramOption
        {
            get; set;
        }
        #endregion

        #region 方法
        void Init(DiagramItem diagramItem);
        void Init(bool initNew);

        void Add(object parameter, bool? isSelected = false);

        void Delete(object parameter);

        bool Next();

        void ClearSelectedItems();

        bool ExecuteShortcut(KeyEventArgs e);

        void SaveThumbnail();
        #endregion

        #region 设置属性
        void SetPropertyValue(SelectableDesignerItemViewModelBase selectable, string propertyName, List<SelectableDesignerItemViewModelBase> items);

        void SetFont(IFontViewModel fontViewModel, string propertyName, List<SelectableDesignerItemViewModelBase> items);

        void SetColor(IColorViewModel colorViewModel, string propertyName, List<SelectableDesignerItemViewModelBase> items);

        void SetSharp(IShapeViewModel shapeViewModel, string propertyName, List<SelectableDesignerItemViewModelBase> items);

        void SetAnimation(IAnimationViewModel animationViewModel, string propertyName, List<SelectableDesignerItemViewModelBase> items);

        void SetQuickItem(IQuickThemeViewModel quickThemeViewModel, string propertyName, List<SelectableDesignerItemViewModelBase> items);

        void LockAction(LockObject lockObject, string propertyName, List<SelectableDesignerItemViewModelBase> items);
        #endregion

        event PropertyChangedEventHandler PropertyChanged;
    }


    public delegate void DiagramEventHandler(object sender, DiagramEventArgs e);

    public class DiagramEventArgs : PropertyChangedEventArgs
    {
        public DiagramEventArgs(string propertyName, object oldValue, object newValue, Guid? id) : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
            Id = id;
        }

        public object OldValue
        {
            get; set;
        }
        public object NewValue
        {
            get; set;
        }
        public Guid? Id
        {
            get; set;
        }

    }
}
