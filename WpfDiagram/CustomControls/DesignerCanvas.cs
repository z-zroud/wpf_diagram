using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;

namespace WpfDiagram.CustomControls
{
    public class DesignerCanvas : Canvas
    {
        #region 属性
        private IDiagramViewModel _viewModel
        {
            get
            {
                return DataContext as IDiagramViewModel;
            }
        }
        private IDiagramServiceProvider _service
        {
            get
            {
                return DiagramServicesProvider.Instance.Provider;
            }
        }
        private ConnectionViewModel _partialConnection;

        private Point? rubberbandSelectionStartPoint = null;

        private Connector _sourceConnector;
        public Connector SourceConnector
        {
            get
            {
                return _sourceConnector;
            }
            set
            {
                if (_sourceConnector != value)
                {
                    _sourceConnector = value;
                    if (_sourceConnector != null)
                    {
                        ConnectorInfoBase sourceDataItem = _sourceConnector.Info;

                        //Rect rectangleBounds = _sourceConnector.TransformToVisual(this).TransformBounds(new Rect(_sourceConnector.RenderSize));
                        //Point point = new Point(rectangleBounds.Left + (rectangleBounds.Width / 2),
                        //                        rectangleBounds.Bottom + (rectangleBounds.Height / 2));

                        Point point = sourceDataItem.MiddlePosition;

                        _partialConnection = new ConnectionViewModel(_viewModel, sourceDataItem, new PartCreatedConnectorInfo(point.X, point.Y), LineDrawMode, RouterMode);

                        _viewModel.Add(_partialConnection);
                        _partialConnection.ZIndex = -1;
                    }
                }
            }
        }

        private Connector _sinkConnector;
        public Connector SinkConnector
        {
            get
            {
                return _sinkConnector;
            }
            set
            {
                if (_sinkConnector != null)
                {
                    _sinkConnector.Info.DisableAttachTo = false;
                }
                if (_sinkConnector != value)
                {
                    _sinkConnector = value;
                }
            }
        }

        private BlockItemsContainer _sourceItemsContainer;
        public BlockItemsContainer SourceItemsContainer
        {
            get
            {
                return _sourceItemsContainer;
            }
            set
            {
                if (_sourceItemsContainer != value)
                {
                    _sourceItemsContainer = value;
                    if (_sourceItemsContainer != null)
                    {
                        BlockItemsContainerInfo sourceContainerInfo = _sourceItemsContainer.Info;

                        sourceContainerInfo.DataItem.RemoveChild(_sourceItemsContainer.DragObject, sourceContainerInfo);

                        EnterMove();
                    }
                }
            }
        }

        private DrawMode DrawMode
        {
            get
            {
                if (_viewModel.DrawModeViewModel != null)
                {
                    return _viewModel.DrawModeViewModel.GetDrawMode();
                }
                else
                {
                    return _service.DrawModeViewModel.GetDrawMode();
                }
            }
        }

        private DrawMode LineDrawMode
        {
            get
            {
                if (_viewModel.DrawModeViewModel != null)
                {
                    return _viewModel.DrawModeViewModel.LineDrawMode;
                }
                else
                {
                    return _service.DrawModeViewModel.LineDrawMode;
                }
            }
        }

        private RouterMode RouterMode
        {
            get
            {
                if (_viewModel.DrawModeViewModel != null)
                {
                    return _viewModel.DrawModeViewModel.LineRouterMode;
                }
                else
                {
                    return _service.DrawModeViewModel.LineRouterMode;
                }
            }
        }

        #region GridCellSize

        public static readonly DependencyProperty GridCellSizeProperty =
            DependencyProperty.Register(nameof(GridCellSize),
                                       typeof(Size),
                                       typeof(DesignerCanvas),
                                       new FrameworkPropertyMetadata(new Size(50, 50), FrameworkPropertyMetadataOptions.AffectsRender));

        public Size GridCellSize
        {
            get
            {
                return (Size)GetValue(GridCellSizeProperty);
            }
            set
            {
                SetValue(GridCellSizeProperty, value);
            }
        }

        #endregion

        #region ShowGrid

        public static readonly DependencyProperty ShowGridProperty =
            DependencyProperty.Register(nameof(ShowGrid),
                                       typeof(bool),
                                       typeof(DesignerCanvas),
                                       new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool ShowGrid
        {
            get
            {
                return (bool)GetValue(ShowGridProperty);
            }
            set
            {
                SetValue(ShowGridProperty, value);
            }
        }

        #endregion

        #region GridColor

        public static readonly DependencyProperty GridColorProperty =
            DependencyProperty.Register(nameof(GridColor),
                                       typeof(Color),
                                       typeof(DesignerCanvas),
                                       new FrameworkPropertyMetadata(Colors.LightGray, FrameworkPropertyMetadataOptions.AffectsRender));

        public Color GridColor
        {
            get
            {
                return (Color)GetValue(GridColorProperty);
            }
            set
            {
                SetValue(GridColorProperty, value);
            }
        }

        #endregion

        #region GridMarginSize 单位mm

        public static readonly DependencyProperty GridMarginSizeProperty =
            DependencyProperty.Register(nameof(GridMarginSize),
                                       typeof(Size),
                                       typeof(DesignerCanvas),
                                       new FrameworkPropertyMetadata(new Size(28, 28), FrameworkPropertyMetadataOptions.AffectsRender));

        public Size GridMarginSize
        {
            get
            {
                return (Size)GetValue(GridMarginSizeProperty);
            }
            set
            {
                SetValue(GridMarginSizeProperty, value);
            }
        }

        #endregion

        #endregion

        #region 初始化
        public DesignerCanvas()
        {
            this.Focusable = true;

            this.Loaded += DesignerCanvas_Loaded;
            this.IsVisibleChanged += DesignerCanvas_IsVisibleChanged;
            this.DataContextChanged += DesignerCanvas_DataContextChanged;
        }

        private void DesignerCanvas_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.OldValue is IDiagramViewModel oldvalue)
            {
                //var image = this.ToBitmap().ToBitmapSource();
                //oldvalue.Thumbnail = new ImageBrush(image) { Stretch = Stretch.Uniform };
            }
            if (e.NewValue is IDiagramViewModel newvalue)
            {
                newvalue.Thumbnail = new VisualBrush(this) { Stretch = Stretch.Uniform };
            }
        }

        private void DesignerCanvas_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                this.Focus();
            }
        }

        protected void DesignerCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            //Mediator.Instance.Register(this);
            this.Focus();
            _service.PropertyChanged += _service_PropertyChanged;
        }

        protected override void OnRender(DrawingContext dc)
        {
            var rect = new Rect(0, 0, RenderSize.Width, RenderSize.Height);
            dc.DrawRectangle(Background, null, rect);
            if (ShowGrid && GridCellSize.Width > 0 && GridCellSize.Height > 0)
                DrawGrid(dc, rect);
        }

        protected virtual void DrawGrid(DrawingContext dc, Rect rect)
        {
            //using .5 forces wpf to draw a single pixel line
            for (var i = GridMarginSize.Height + 0.5; i < rect.Height - GridMarginSize.Height; i += GridCellSize.Height)
                dc.DrawLine(new Pen(new SolidColorBrush(GridColor), 1), new Point(GridMarginSize.Width, i), new Point(rect.Width - GridMarginSize.Width, i));
            dc.DrawLine(new Pen(new SolidColorBrush(GridColor), 1), new Point(GridMarginSize.Width, rect.Height - GridMarginSize.Height), new Point(rect.Width - GridMarginSize.Width, rect.Height - GridMarginSize.Height));

            for (var i = GridMarginSize.Width + 0.5; i < rect.Width - GridMarginSize.Width; i += GridCellSize.Width)
                dc.DrawLine(new Pen(new SolidColorBrush(GridColor), 1), new Point(i, GridMarginSize.Height), new Point(i, rect.Height - GridMarginSize.Height));
            dc.DrawLine(new Pen(new SolidColorBrush(GridColor), 1), new Point(rect.Width - GridMarginSize.Width, GridMarginSize.Height), new Point(rect.Width - GridMarginSize.Width, rect.Height - GridMarginSize.Height));
        }
        #endregion

        #region Format/Move
        private void _service_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is IDrawModeViewModel)
            {
                if (e.PropertyName == nameof(CursorMode))
                {
                    if (_service.DrawModeViewModel.CursorMode == CursorMode.Format)
                    {
                        EnterFormat();
                    }
                    else if (_service.DrawModeViewModel.CursorMode == CursorMode.Move)
                    {
                        EnterMove();
                    }
                    else if (_service.DrawModeViewModel.CursorMode == CursorMode.Exit)
                    {
                        ExitCursor();
                    }
                }
                else if (e.PropertyName == nameof(_service.DrawModeViewModel.DrawingDrawMode))
                {
                    if (_service.DrawModeViewModel.DrawingDrawMode == DrawMode.ColorPicker)
                    {
                        EnterColorPicker();
                    }
                }
            }
        }

        private void EnterFormat()
        {
            StreamResourceInfo sri = Application.GetResourceStream(new Uri("pack://application:,,,/AIStudio.Wpf.DiagramDesigner;component/Images/FormatPainter.cur", UriKind.RelativeOrAbsolute));
            this.Cursor = new Cursor(sri.Stream);

            foreach (SelectableDesignerItemViewModelBase item in _viewModel.Items)
            {
                item.IsHitTestVisible = false;
            }
        }

        private void EnterMove()
        {
            this.Cursor = Cursors.SizeAll;
            foreach (SelectableDesignerItemViewModelBase item in _viewModel.Items)
            {
                item.IsHitTestVisible = false;
            }
        }

        private void EnterColorPicker()
        {

            // create rubberband adorner
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (adornerLayer != null)
            {
                DrawingRubberbandAdorner adorner = new DrawingRubberbandAdorner(this, new Point());
                if (adorner != null)
                {
                    adornerLayer.Add(adorner);
                }
            }
        }

        private void ExitCursor()
        {
            this.Cursor = Cursors.Arrow;
            foreach (SelectableDesignerItemViewModelBase item in _viewModel.Items)
            {
                item.IsHitTestVisible = true;
            }
            _service.DrawModeViewModel.CursorMode = CursorMode.Normal;
        }

        private void Format(SelectableDesignerItemViewModelBase source, SelectableDesignerItemViewModelBase target)
        {
            CopyHelper.CopyPropertyValue(source.ColorViewModel, target.ColorViewModel);
            CopyHelper.CopyPropertyValue(source.FontViewModel, target.FontViewModel);
            CopyHelper.CopyPropertyValue(source.ShapeViewModel, target.ShapeViewModel);
            CopyHelper.CopyPropertyValue(source.AnimationViewModel, target.AnimationViewModel);
        }
        #endregion

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (_viewModel.IsReadOnly) return;

            if (_service.DrawModeViewModel.CursorMode == CursorMode.Format)
            {
                var element = (e.OriginalSource as FrameworkElement);
                if (element.DataContext is SelectableDesignerItemViewModelBase target)
                {
                    Format(_viewModel.SelectedItems.FirstOrDefault(), target);
                    return;
                }

                ExitCursor();
            }
            else if (_service.DrawModeViewModel.CursorMode == CursorMode.Move)
            {
                ExitCursor();
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //if we are source of event, we are rubberband selecting
                if (e.Source == this)
                {
                    // in case that this click is the start for a 
                    // drag operation we cache the start point
                    Point currentPoint = e.GetPosition(this);
                    rubberbandSelectionStartPoint = currentPoint;

                    if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    {
                        _viewModel.ClearSelectedItems();

                    }

                    if (_service.DrawModeViewModel.SharpDrawModeSelected ||
                        (_service.DrawModeViewModel.DrawingDrawModeSelected && _service.DrawModeViewModel.DrawingDrawMode != DrawMode.Select))
                    {
                        // create rubberband adorner
                        AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                        if (adornerLayer != null)
                        {
                            DrawingRubberbandAdorner adorner = new DrawingRubberbandAdorner(this, currentPoint);
                            if (adorner != null)
                            {
                                adornerLayer.Add(adorner);
                            }
                        }
                    }
                    else if (_service.DrawModeViewModel.LineDrawModeSelected)//画线模式,可以不命中实体
                    {
                        if (SourceConnector == null)
                        {
                            //新建一个Part连接点
                            SourceConnector = new Connector() { Content = new PartCreatedConnectorInfo(currentPoint.X, currentPoint.Y), Tag = "虚拟的连接点" };
                        }
                    }
                    e.Handled = true;
                }
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //var focusedElement = Keyboard.FocusedElement;
            //Debug.WriteLine("focusedElement：" + focusedElement?.ToString());

            base.OnMouseMove(e);

            if (_viewModel.IsReadOnly) return;

            Point currentPoint = e.GetPosition(this);
            _viewModel.CurrentPoint = new Point(ScreenHelper.WidthToMm(currentPoint.X), ScreenHelper.WidthToMm(currentPoint.Y));
            var point = CursorPointManager.GetCursorPosition();
            _viewModel.CurrentColor = ColorPickerManager.GetColor(point.X, point.Y);

            //移动
            if (_service.DrawModeViewModel.CursorMode == CursorMode.Move)
            {
                _viewModel.SelectedItems.OfType<DesignerItemViewModelBase>().ToList().ForEach(p => {
                    p.Left = currentPoint.X;
                    p.Top = currentPoint.Y;
                });
                return;
            }

            if (SourceConnector != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    _partialConnection.SinkConnectorInfo = new PartCreatedConnectorInfo(currentPoint.X, currentPoint.Y);

                    SinkConnector = HitTesting(currentPoint);
                    if (SinkConnector?.Info?.CanAttachTo(SourceConnector?.Info) == false)
                    {
                        SinkConnector.Info.DisableAttachTo = true;
                    }

                    if (_viewModel.DiagramOption.SnappingOption.EnableSnapping)
                    {
                        var nearPort = _viewModel.FindNearPortToAttachTo(_partialConnection);
                        if (nearPort != null)
                        {
                            _partialConnection.SinkConnectorInfo = nearPort;
                        }
                    }
                }
            }
            else if (SourceItemsContainer != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    SourceItemsContainer.DragObject.Left = currentPoint.X - SourceItemsContainer.DragOffset.X;
                    SourceItemsContainer.DragObject.Top = currentPoint.Y - SourceItemsContainer.DragOffset.Y;

                    (_viewModel as IBlockDiagramViewModel).PreviewNearBlock(new System.Collections.Generic.List<BlockDesignerItemViewModel> { SourceItemsContainer.DragObject });
                }
            }
            else
            {
                // if mouse button is not pressed we have no drag operation, ...
                if (e.LeftButton != MouseButtonState.Pressed)
                    rubberbandSelectionStartPoint = null;

                // ... but if mouse button is pressed and start
                // point value is set we do have one
                if (this.rubberbandSelectionStartPoint.HasValue)
                {
                    // create rubberband adorner
                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                    if (adornerLayer != null)
                    {
                        RubberbandAdorner adorner = new RubberbandAdorner(this, rubberbandSelectionStartPoint);
                        if (adorner != null)
                        {
                            adornerLayer.Add(adorner);
                        }
                    }
                }
            }

            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (_viewModel.IsReadOnly) return;

            Mediator.Instance.NotifyColleagues<bool>("DoneDrawingMessage", true);

            if (SourceConnector != null)
            {
                ConnectorInfoBase sourceDataItem = SourceConnector.Info;
                if (SinkConnector != null && SinkConnector.Info?.DisableAttachTo == false)
                {
                    ConnectorInfoBase sinkDataItem = SinkConnector.Info;

                    _viewModel.Delete(_partialConnection);
                    _viewModel.AddCommand.Execute(new ConnectionViewModel(_viewModel, sourceDataItem, sinkDataItem, LineDrawMode, RouterMode));
                }
                else if (_partialConnection.IsFullConnection)//自动连接模式
                {
                    _viewModel.ClearAttachTo();
                }
                else if (_service.DrawModeViewModel.LineDrawModeSelected)
                {
                    Point currentPoint = e.GetPosition(this);
                    ConnectorInfoBase sinkDataItem = new PartCreatedConnectorInfo(currentPoint.X, currentPoint.Y);

                    _viewModel.Delete(_partialConnection);
                    _viewModel.AddCommand.Execute(new ConnectionViewModel(_viewModel, sourceDataItem, sinkDataItem, LineDrawMode, RouterMode));
                }
                else
                {
                    //Need to remove last item as we did not finish drawing the path
                    _viewModel.Delete(_partialConnection);
                }
            }
            else if (SourceItemsContainer != null)
            {
                (_viewModel as IBlockDiagramViewModel).FinishNearBlock(new System.Collections.Generic.List<BlockDesignerItemViewModel> { SourceItemsContainer.DragObject });
                ExitCursor();
            }

            SourceConnector = null;
            SinkConnector = null;
            _partialConnection = null;

            SourceItemsContainer = null;

            _service.DrawModeViewModel.ResetDrawMode();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (_viewModel.IsReadOnly) return;

            e.Handled = _viewModel.ExecuteShortcut(e);
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            if (Keyboard.IsKeyDown(Key.LeftCtrl) == false
               && Keyboard.IsKeyDown(Key.RightCtrl) == false)
            {
                return;
            }

            var newZoomValue = _viewModel.ZoomValue + (e.Delta > 0 ? 0.1 : -0.1);

            _viewModel.ZoomValue = Math.Max(Math.Min(newZoomValue, _viewModel.MaximumZoomValue), _viewModel.MinimumZoomValue);

            e.Handled = true;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();
            foreach (UIElement element in this.InternalChildren)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                //measure desired size for each child
                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            // add margin 
            size.Width += 10;
            size.Height += 10;

            return size;
        }

        private Connector HitTesting(Point hitPoint)
        {
            DependencyObject hitObject = this.InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null &&
                    hitObject.GetType() != typeof(DesignerCanvas))
            {
                if (hitObject is Connector connector)
                {
                    return connector;
                }
                hitObject = VisualTreeHelper.GetParent(hitObject);
            }

            return null;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            DragObject dragObject = e.Data.GetData(typeof(DragObject)) as DragObject;
            if (dragObject != null && (dragObject.ContentType == typeof(BlockDesignerItemViewModel) || dragObject.ContentType.IsSubclassOf(typeof(BlockDesignerItemViewModel))))
            {
                var position = e.GetPosition(this);

                BlockDesignerItemViewModel itemBase = Activator.CreateInstance(dragObject.ContentType) as BlockDesignerItemViewModel;
                itemBase.Text = dragObject.Text;
                itemBase.Icon = dragObject.Icon;
                itemBase.ColorViewModel = CopyHelper.Mapper(dragObject.ColorViewModel);
                if (dragObject.DesiredSize != null)
                {
                    itemBase.ItemWidth = dragObject.DesiredSize.Value.Width;
                    itemBase.ItemHeight = dragObject.DesiredSize.Value.Height;
                }
                if (dragObject.DesiredMinSize != null)
                {
                    itemBase.MinItemWidth = dragObject.DesiredMinSize.Value.Width;
                    itemBase.MinItemHeight = dragObject.DesiredMinSize.Value.Height;
                }
                itemBase.Left = Math.Max(0, position.X - itemBase.GetItemWidth() / 2);
                itemBase.Top = Math.Max(0, position.Y - itemBase.GetItemHeight() / 2);

                (_viewModel as IBlockDiagramViewModel)?.PreviewNearBlock(new System.Collections.Generic.List<BlockDesignerItemViewModel> { itemBase });
            }

            base.OnDragOver(e);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (_viewModel.IsReadOnly) return;

            DragObject dragObject = e.Data.GetData(typeof(DragObject)) as DragObject;
            if (dragObject != null)
            {
                _viewModel.ClearSelectedItems();
                Point position = e.GetPosition(this);
                if (dragObject.DesignerItem is SerializableObject serializableObject)
                {
                    var designerItems = serializableObject.ToObject();
                    var minleft = designerItems.OfType<DesignerItemViewModelBase>().Min(p => p.Left);
                    var mintop = designerItems.OfType<DesignerItemViewModelBase>().Min(p => p.Top);
                    var maxright = designerItems.OfType<DesignerItemViewModelBase>().Max(p => p.Left + p.GetItemWidth());
                    var maxbottom = designerItems.OfType<DesignerItemViewModelBase>().Max(p => p.Top + p.GetItemHeight());
                    var itemswidth = maxright - minleft;
                    var itemsheight = maxbottom - mintop;

                    foreach (var item in designerItems.OfType<DesignerItemViewModelBase>())
                    {
                        item.Left += position.X - itemswidth / 2;
                        item.Top += position.Y - itemsheight / 2;
                    }
                    _viewModel.AddCommand.Execute(designerItems);
                }
                else if (dragObject.DesignerItem is SerializableItem serializableItem)
                {
                    Type type = TypeHelper.GetType(serializableItem.ModelTypeName);
                    DesignerItemViewModelBase itemBase = Activator.CreateInstance(type, _viewModel, serializableItem, ".json") as DesignerItemViewModelBase;
                    itemBase.Left = Math.Max(0, position.X - itemBase.GetItemWidth() / 2);
                    itemBase.Top = Math.Max(0, position.Y - itemBase.GetItemHeight() / 2);
                    if (dragObject.DesiredSize != null)
                    {
                        itemBase.ItemWidth = dragObject.DesiredSize.Value.Width;
                        itemBase.ItemHeight = dragObject.DesiredSize.Value.Height;
                    }
                    if (dragObject.DesiredMinSize != null)
                    {
                        itemBase.MinItemWidth = dragObject.DesiredMinSize.Value.Width;
                        itemBase.MinItemHeight = dragObject.DesiredMinSize.Value.Height;
                    }
                    _viewModel.AddCommand.Execute(itemBase);

                    if (itemBase is BlockDesignerItemViewModel block)
                    {
                        (_viewModel as IBlockDiagramViewModel).FinishNearBlock(new System.Collections.Generic.List<BlockDesignerItemViewModel> { block });
                    }
                }
                else
                {
                    DesignerItemViewModelBase itemBase = null;
                    if (dragObject.DesignerItem is DesignerItemBase)
                    {
                        itemBase = Activator.CreateInstance(dragObject.ContentType, _viewModel, dragObject.DesignerItem) as DesignerItemViewModelBase;
                    }
                    else
                    {
                        itemBase = Activator.CreateInstance(dragObject.ContentType) as DesignerItemViewModelBase;
                        if (!string.IsNullOrEmpty(dragObject.Text))
                            itemBase.Text = dragObject.Text;
                        if (!string.IsNullOrEmpty(dragObject.Icon))
                            itemBase.Icon = dragObject.Icon;
                        itemBase.ColorViewModel = CopyHelper.Mapper(dragObject.ColorViewModel);
                        if (dragObject.DesiredSize != null)
                        {
                            itemBase.ItemWidth = dragObject.DesiredSize.Value.Width;
                            itemBase.ItemHeight = dragObject.DesiredSize.Value.Height;
                        }
                        if (dragObject.DesiredMinSize != null)
                        {
                            itemBase.MinItemWidth = dragObject.DesiredMinSize.Value.Width;
                            itemBase.MinItemHeight = dragObject.DesiredMinSize.Value.Height;
                        }

                    }
                    itemBase.Left = Math.Max(0, position.X - itemBase.GetItemWidth() / 2);
                    itemBase.Top = Math.Max(0, position.Y - itemBase.GetItemHeight() / 2);
                    _viewModel.AddCommand.Execute(itemBase);

                    if (itemBase is BlockDesignerItemViewModel block)
                    {
                        (_viewModel as IBlockDiagramViewModel).FinishNearBlock(new System.Collections.Generic.List<BlockDesignerItemViewModel> { block });
                    }
                }
            }
            else
            {
                var dragFile = e.Data.GetData(DataFormats.FileDrop);
                if (dragFile != null && dragFile is string[] files)
                {
                    foreach (var file in files)
                    {
                        _viewModel.ClearSelectedItems();
                        Point position = e.GetPosition(this);
                        ImageItemViewModel itemBase = new ImageItemViewModel();
                        itemBase.Icon = file;
                        itemBase.Suffix = System.IO.Path.GetExtension(itemBase.Icon).ToLower();
                        itemBase.InitWidthAndHeight();
                        itemBase.AutoSize();

                        itemBase.Left = Math.Max(0, position.X - itemBase.GetItemWidth() / 2);
                        itemBase.Top = Math.Max(0, position.Y - itemBase.GetItemHeight() / 2);
                        _viewModel.AddCommand.Execute(itemBase);
                    }
                }
            }
            e.Handled = true;

            this.Focus();
        }
    }
}
