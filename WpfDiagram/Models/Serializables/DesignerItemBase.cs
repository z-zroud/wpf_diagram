using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDiagram.Models.Serializables
{
    public abstract class DesignerItemViewModelBase : SelectableDesignerItemViewModelBase
    {
        public DesignerItemViewModelBase() : this(null)
        {

        }

        public DesignerItemViewModelBase(IDiagramViewModel root) : base(root)
        {
            ShapeDefiner = Shapes.Rectangle;
        }

        public DesignerItemViewModelBase(IDiagramViewModel root, SelectableItemBase designer) : base(root, designer)
        {
            ShapeDefiner = Shapes.Rectangle;
        }

        public DesignerItemViewModelBase(IDiagramViewModel root, SerializableItem serializableItem, string serializableType) : base(root, serializableItem, serializableType)
        {
            ShapeDefiner = Shapes.Rectangle;
        }

        public override SelectableItemBase GetSerializableObject()
        {
            return new DesignerItemBase(this);
        }

        protected override void InitNew()
        {
            AddConnector(new FullyCreatedConnectorInfo(this.Root, this, ConnectorOrientation.Top));
            AddConnector(new FullyCreatedConnectorInfo(this.Root, this, ConnectorOrientation.Bottom));
            AddConnector(new FullyCreatedConnectorInfo(this.Root, this, ConnectorOrientation.Left));
            AddConnector(new FullyCreatedConnectorInfo(this.Root, this, ConnectorOrientation.Right));
        }

        protected override void LoadDesignerItemViewModel(SelectableItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(designerbase);

            if (designerbase is DesignerItemBase designer)
            {
                this.PhysicalLeft = designer.PhysicalLeft;
                this.PhysicalTop = designer.PhysicalTop;
                this.Angle = designer.Angle;
                this.ScaleX = designer.ScaleX;
                this.ScaleY = designer.ScaleY;
                this.PhysicalItemWidth = designer.PhysicalItemWidth;
                this.PhysicalItemHeight = designer.PhysicalItemHeight;
                this.MinItemWidth = designer.MinItemWidth;
                this.MinItemHeight = designer.MinItemHeight;
                this.Icon = designer.Icon;
                this.CornerRadius = designer.CornerRadius;
                this.BorderThickness = designer.BorderThickness;
                if (designer.Connectors != null)
                {
                    foreach (var connector in designer.Connectors)
                    {
                        FullyCreatedConnectorInfo fullyCreatedConnectorInfo = new FullyCreatedConnectorInfo(this.Root, this, connector);
                        AddConnector(fullyCreatedConnectorInfo);
                    }
                }
            }
        }
        #region 

        public FullyCreatedConnectorInfo FirstConnector
        {
            get
            {
                return Connectors?.FirstOrDefault();
            }
        }

        public FullyCreatedConnectorInfo TopConnector
        {
            get
            {
                return Connectors?.FirstOrDefault(p => p.Orientation == ConnectorOrientation.Top);
            }
        }

        public FullyCreatedConnectorInfo BottomConnector
        {
            get
            {
                return Connectors?.FirstOrDefault(p => p.Orientation == ConnectorOrientation.Bottom);
            }
        }

        public FullyCreatedConnectorInfo LeftConnector
        {
            get
            {
                return Connectors?.FirstOrDefault(p => p.Orientation == ConnectorOrientation.Left);
            }
        }

        public FullyCreatedConnectorInfo RightConnector
        {
            get
            {
                return Connectors?.FirstOrDefault(p => p.Orientation == ConnectorOrientation.Right);
            }
        }

        public FullyCreatedConnectorInfo TopLeftConnector
        {
            get
            {
                return Connectors?.FirstOrDefault(p => p.Orientation == ConnectorOrientation.TopLeft);
            }
        }

        public FullyCreatedConnectorInfo TopRightConnector
        {
            get
            {
                return Connectors?.FirstOrDefault(p => p.Orientation == ConnectorOrientation.TopRight);
            }
        }

        public FullyCreatedConnectorInfo BottomLeftConnector
        {
            get
            {
                return Connectors?.FirstOrDefault(p => p.Orientation == ConnectorOrientation.BottomLeft);
            }
        }

        public FullyCreatedConnectorInfo BottomRightConnector
        {
            get
            {
                return Connectors?.FirstOrDefault(p => p.Orientation == ConnectorOrientation.BottomRight);
            }
        }

        private FullyCreatedConnectorInfo _portlessConnector;
        public FullyCreatedConnectorInfo PortlessConnector
        {
            get
            {
                if (_portlessConnector == null)
                    _portlessConnector = new FullyCreatedConnectorInfo(this.Root, this, ConnectorOrientation.None) { IsPortless = true };

                return _portlessConnector;
            }
        }

        public Style ConnectorStyle
        {
            get; set;
        }

        public ShapeDefiner ShapeDefiner
        {
            get;
        }

        public virtual PointBase MiddlePosition
        {
            get
            {
                return GetBounds().Center;
            }
        }

        private string _icon;
        [CanDo]
        public string Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                SetProperty(ref _icon, value);
            }
        }

        private double _itemWidth = 65;
        [CanDo]
        [Browsable(true)]
        public double ItemWidth
        {
            get
            {
                return _itemWidth;
            }
            set
            {
                if (value <= 0) return;
                if (SetProperty(ref _itemWidth, value))
                {
                    RaisePropertyChanged(nameof(PhysicalItemWidth));
                }
            }
        }

        private double _itemHeight = 65;
        [CanDo]
        [Browsable(true)]
        public double ItemHeight
        {
            get
            {
                return _itemHeight;
            }
            set
            {
                if (value <= 0) return;
                if (SetProperty(ref _itemHeight, value))
                {
                    RaisePropertyChanged(nameof(PhysicalItemHeight));
                }
            }
        }

        [DisplayName("ItemWidth(mm)")]
        [Browsable(true)]
        public double PhysicalItemWidth
        {
            get
            {
                return ScreenHelper.WidthToMm(ItemWidth);
            }
            set
            {
                ItemWidth = ScreenHelper.MmToWidth(value);
            }
        }

        [DisplayName("ItemHeight(mm)")]
        [Browsable(true)]
        public double PhysicalItemHeight
        {
            get
            {
                return ScreenHelper.WidthToMm(ItemHeight);
            }
            set
            {
                ItemHeight = ScreenHelper.MmToWidth(value);
            }
        }

        [CanDo]
        public SizeBase Size
        {
            get
            {
                return new SizeBase(GetItemWidth(), GetItemHeight());
            }
            set
            {
                ItemWidth = value.Width;
                ItemHeight = value.Height;
            }
        }

        private double _connectorMargin = -4;
        public double ConnectorMargin
        {
            get
            {
                return _connectorMargin;
            }
            set
            {
                SetProperty(ref _connectorMargin, value);
            }
        }

        private bool _showConnectors = false;
        public bool ShowConnectors
        {
            get
            {
                return _showConnectors;
            }
            set
            {
                if (SetProperty(ref _showConnectors, value))
                {
                    foreach (var connector in Connectors)
                    {
                        connector.ShowConnectors = value;
                    }
                }
            }
        }

        private bool _showResize = true;
        public bool ShowResize
        {
            get
            {
                return _showResize;
            }
            set
            {
                SetProperty(ref _showResize, value);
            }
        }

        private bool _showRotate = false;
        public bool ShowRotate
        {
            get
            {
                return _showRotate;
            }
            set
            {
                SetProperty(ref _showRotate, value);
            }
        }

        public bool ShowArrow { get; set; } = true;

        private bool alwayForResized;
        public bool AlwayForResized
        {
            get
            {
                return alwayForResized;
            }
            set
            {
                SetProperty(ref alwayForResized, value);
            }
        }

        private bool enabledForConnection = true;
        public bool EnabledForConnection
        {
            get
            {
                return enabledForConnection;
            }
            set
            {
                SetProperty(ref enabledForConnection, value);
            }
        }

        protected double _left;
        [CanDo]
        [Browsable(true)]
        public double Left
        {
            get
            {
                return _left;
            }
            set
            {
                if (Root != null && BeyondBoundary > 0)
                {
                    if (value + GetItemWidth() < BeyondBoundary)
                    {
                        value = BeyondBoundary - GetItemWidth();
                    }
                    else if (value > Root.DiagramOption.LayoutOption.PageSize.Width - BeyondBoundary)
                    {
                        value = Root.DiagramOption.LayoutOption.PageSize.Width - BeyondBoundary;
                    }
                }
                SetProperty(ref _left, value);
            }
        }

        private double _top;
        [CanDo]
        [Browsable(true)]
        public double Top
        {
            get
            {
                return _top;
            }
            set
            {
                if (Root != null && BeyondBoundary > 0)
                {
                    if (value + GetItemHeight() < BeyondBoundary)
                    {
                        value = BeyondBoundary - GetItemHeight();
                    }
                    else if (value > Root.DiagramOption.LayoutOption.PageSize.Height - BeyondBoundary)
                    {
                        value = Root.DiagramOption.LayoutOption.PageSize.Height - BeyondBoundary;
                    }
                }
                SetProperty(ref _top, value);
            }
        }

        [DisplayName("Left(mm)")]
        [Browsable(true)]
        public double PhysicalLeft
        {
            get
            {
                return ScreenHelper.WidthToMm(Left);
            }
            set
            {
                Left = ScreenHelper.MmToWidth(value);
            }
        }

        [DisplayName("Top(mm)")]
        [Browsable(true)]
        public double PhysicalTop
        {
            get
            {
                return ScreenHelper.WidthToMm(Top);
            }
            set
            {
                Top = ScreenHelper.MmToWidth(value);
            }
        }

        public PointBase Position
        {
            get
            {
                return new PointBase(Left, Top);
            }
        }

        [CanDo]
        public PointBase TopLeft
        {
            get
            {
                return new PointBase(Left, Top);
            }
            set
            {
                Left = value.X;
                Top = value.Y;
                RaisePropertyChanged(nameof(TopLeft));
            }
        }

        public PointBase RightBottom
        {
            get
            {
                return new Point(Left + GetItemWidth(), Top + GetItemHeight());
            }
        }

        private double _angle;
        [CanDo]
        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                SetProperty(ref _angle, value);
            }
        }

        private double _scaleX = 1;
        [CanDo]
        public double ScaleX
        {
            get
            {
                return _scaleX;
            }
            set
            {
                SetProperty(ref _scaleX, value);
            }
        }

        private double _scaleY = 1;
        [CanDo]
        public double ScaleY
        {
            get
            {
                return _scaleY;
            }
            set
            {
                SetProperty(ref _scaleY, value);
            }
        }

        private double _margin;

        public double Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                SetProperty(ref _margin, value);
            }
        }

        private CornerRadius _cornerRadius = new CornerRadius(3);
        public CornerRadius CornerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                SetProperty(ref _cornerRadius, value);
            }
        }

        private Thickness _borderThickness = new Thickness(1);
        public Thickness BorderThickness
        {
            get
            {
                return _borderThickness;
            }
            set
            {
                SetProperty(ref _borderThickness, value);
            }
        }

        private double _minItemWidth;
        public double MinItemWidth
        {
            get
            {
                return _minItemWidth;
            }
            set
            {
                SetProperty(ref _minItemWidth, value);
            }
        }

        private double _minItemHeight;
        public double MinItemHeight
        {
            get
            {
                return _minItemHeight;
            }
            set
            {
                SetProperty(ref _minItemHeight, value);
            }
        }

        private double _actualItemWidth;
        public double ActualItemWidth
        {
            get
            {
                return _actualItemWidth;
            }
            set
            {
                SetProperty(ref _actualItemWidth, value);
            }
        }

        private double _actualItemHeight;
        public double ActualItemHeight
        {
            get
            {
                return _actualItemHeight;
            }
            set
            {
                SetProperty(ref _actualItemHeight, value);
            }
        }

        public double BeyondBoundary
        {
            get; set;
        } = -1;

        /// <summary>
        /// 连接点是否可以按偏移自定义
        /// </summary>
        public bool IsInnerConnector
        {
            get; set;
        }

        protected ObservableCollection<FullyCreatedConnectorInfo> connectors = new ObservableCollection<FullyCreatedConnectorInfo>();
        public ObservableCollection<FullyCreatedConnectorInfo> Connectors
        {
            get
            {
                return connectors;
            }
        }

        protected ObservableCollection<CinchMenuItem> menuOptions;
        public IEnumerable<CinchMenuItem> MenuOptions
        {
            get
            {
                return menuOptions;
            }
        }

        public bool ShowMenuOptions
        {
            get
            {
                if (MenuOptions == null || MenuOptions.Count() == 0)
                    return false;
                else
                    return true;
            }
        }

        public IObservable<NotifyCollectionChangedEventArgs> WhenConnectorsChanged
        {
            get
            {
                return Observable
                    .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                        h => this.Connectors.CollectionChanged += h,
                        h => this.Connectors.CollectionChanged -= h)
                    .Select(x => x.EventArgs);
            }
        }

        #endregion

        #region 方法
        public void AddConnector(FullyCreatedConnectorInfo connector)
        {
            if (!Connectors.Contains(connector))
            {
                Connectors.Add(connector);
                if (!double.IsNaN(connector.ConnectorWidth))
                    ConnectorMargin = 0 - connector.ConnectorWidth / 2;
            }
        }

        public void RemoveConnector(FullyCreatedConnectorInfo connector)
        {
            if (Connectors.Contains(connector))
            {
                Connectors.Remove(connector);
            }
        }

        public virtual void ClearConnectors()
        {
            Connectors.Clear();
        }

        public void SetCellAlignment()
        {
            if (!(this is TextDesignerItemViewModel))
            {
                if (Root.DiagramOption.LayoutOption.CellHorizontalAlignment == CellHorizontalAlignment.Center)
                {
                    if (Root.DiagramOption.LayoutOption.GridCellSize.Width > this.GetItemWidth())
                    {
                        this.Left = (int)(this.Left / Root.DiagramOption.LayoutOption.GridCellSize.Width) * Root.DiagramOption.LayoutOption.GridCellSize.Width + Root.DiagramOption.LayoutOption.GridMarginSize.Width + (Root.DiagramOption.LayoutOption.GridCellSize.Width - this.GetItemWidth()) / 2;
                    }
                }
                else if (Root.DiagramOption.LayoutOption.CellHorizontalAlignment == CellHorizontalAlignment.Left)
                {
                    this.Left = (int)(this.Left / Root.DiagramOption.LayoutOption.GridCellSize.Width) * Root.DiagramOption.LayoutOption.GridCellSize.Width + Root.DiagramOption.LayoutOption.GridMarginSize.Width;
                }
                else if (Root.DiagramOption.LayoutOption.CellHorizontalAlignment == CellHorizontalAlignment.Right)
                {
                    if (Root.DiagramOption.LayoutOption.GridCellSize.Width > this.GetItemWidth())
                    {
                        this.Left = (int)(this.Left / Root.DiagramOption.LayoutOption.GridCellSize.Width) * Root.DiagramOption.LayoutOption.GridCellSize.Width + Root.DiagramOption.LayoutOption.GridMarginSize.Width + (Root.DiagramOption.LayoutOption.GridCellSize.Width - this.GetItemWidth());
                    }
                }

                if (Root.DiagramOption.LayoutOption.CellVerticalAlignment == CellVerticalAlignment.Center)
                {
                    if (Root.DiagramOption.LayoutOption.GridCellSize.Height > this.GetItemHeight())
                    {
                        this.Top = (int)(this.Top / Root.DiagramOption.LayoutOption.GridCellSize.Height) * Root.DiagramOption.LayoutOption.GridCellSize.Height + Root.DiagramOption.LayoutOption.GridMarginSize.Height + (Root.DiagramOption.LayoutOption.GridCellSize.Height - this.GetItemHeight()) / 2;
                    }
                }
                else if (Root.DiagramOption.LayoutOption.CellVerticalAlignment == CellVerticalAlignment.Top)
                {
                    this.Top = (int)(this.Top / Root.DiagramOption.LayoutOption.GridCellSize.Height) * Root.DiagramOption.LayoutOption.GridCellSize.Height + Root.DiagramOption.LayoutOption.GridMarginSize.Height;
                }
                else if (Root.DiagramOption.LayoutOption.CellVerticalAlignment == CellVerticalAlignment.Bottom)
                {
                    if (Root.DiagramOption.LayoutOption.GridCellSize.Height > this.GetItemHeight())
                    {
                        this.Top = (int)(this.Top / Root.DiagramOption.LayoutOption.GridCellSize.Height) * Root.DiagramOption.LayoutOption.GridCellSize.Height + Root.DiagramOption.LayoutOption.GridMarginSize.Height + (Root.DiagramOption.LayoutOption.GridCellSize.Height - this.GetItemHeight());
                    }
                }
            }
        }

        public void RaiseTopLeft()
        {
            this.RaisePropertyChanged(nameof(TopLeft), new PointBase(GetOldValue<double>(nameof(Left)), GetOldValue<double>(nameof(Top))), TopLeft);
        }

        public void RaiseItemWidthHeight()
        {
            this.RaisePropertyChanged(nameof(Size), new SizeBase(GetOldValue<double>(nameof(ItemWidth)), GetOldValue<double>(nameof(ItemHeight))), Size);
        }

        public void RaiseAngle()
        {
            this.RaisePropertyChanged(nameof(Angle), GetOldValue<double>(nameof(Angle)), Angle);
        }

        public FullyCreatedConnectorInfo GetFullConnectorInfo(Guid connectorId, ConnectorOrientation connectorOrientation, double xRatio, double yRatio, bool isInnerPoint, bool isPortless)
        {
            if (isInnerPoint)
            {
                var connector = Connectors.FirstOrDefault(p => p.XRatio == xRatio && p.YRatio == yRatio);
                if (connector == null)
                {
                    connector = Connectors.FirstOrDefault(p => p.Id == connectorId);
                }
                return connector;
            }
            else if (isPortless)
            {
                return this.PortlessConnector;
            }
            else
            {
                switch (connectorOrientation)
                {
                    case ConnectorOrientation.Left:
                        return this.LeftConnector;
                    case ConnectorOrientation.TopLeft:
                        return this.TopLeftConnector;
                    case ConnectorOrientation.Top:
                        return this.TopConnector;
                    case ConnectorOrientation.TopRight:
                        return this.TopRightConnector;
                    case ConnectorOrientation.Right:
                        return this.RightConnector;
                    case ConnectorOrientation.BottomRight:
                        return this.BottomRightConnector;
                    case ConnectorOrientation.Bottom:
                        return this.BottomConnector;
                    case ConnectorOrientation.BottomLeft:
                        return this.BottomLeftConnector;

                    default:
                        throw new InvalidOperationException(
                            string.Format("Found invalid persisted Connector Orientation for Connector Id: {0}", connectorId));
                }
            }
        }

        public RectangleBase GetBounds(bool includePorts = false)
        {
            if (!includePorts)
                return new RectangleBase(Position, Size);

            var leftPort = LeftConnector;
            var topPort = TopConnector;
            var rightPort = RightConnector;
            var bottomPort = BottomConnector;

            var left = leftPort == null ? Position.X : Math.Min(Position.X, leftPort.Position.X);
            var top = topPort == null ? Position.Y : Math.Min(Position.Y, topPort.Position.Y);
            var right = rightPort == null ? Position.X + GetItemWidth() :
                Math.Max(rightPort.Position.X + rightPort.ConnectorWidth, Position.X + GetItemWidth());
            var bottom = bottomPort == null ? Position.Y + ItemHeight :
                Math.Max(bottomPort.Position.Y + bottomPort.ConnectorHeight, Position.Y + GetItemHeight());

            return new RectangleBase(left, top, right, bottom, true);
        }

        public virtual double GetItemWidth()
        {
            return double.IsNaN(ItemWidth) ? Math.Max(ActualItemWidth, MinItemWidth) : ItemWidth;
        }

        public virtual double GetItemHeight()
        {
            return double.IsNaN(ItemHeight) ? Math.Max(ActualItemHeight, MinItemHeight) : ItemHeight;
        }

        public IShape GetShape() => ShapeDefiner(this);

        public override string ToString()
        {
            return $"{ParentId}-{Id}-{Name}-{Text}-({Left},{Top},{ItemWidth},{ItemHeight})";
        }
        #endregion
    }
}
