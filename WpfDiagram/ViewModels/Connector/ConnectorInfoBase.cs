using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfDiagram.Enums;

namespace WpfDiagram.ViewModels.Connector
{
    public abstract class ConnectorInfoBase : SelectableViewModelBase, IAttachTo
    {
        public ConnectorInfoBase(ConnectorOrientation orientation) : this(null, orientation)
        {

        }

        public ConnectorInfoBase(IDiagramViewModel root, ConnectorOrientation orientation) : base(root)
        {
            this.Orientation = orientation;
        }

        public ConnectorInfoBase(IDiagramViewModel root, SelectableItemBase designer) : base(root, designer)
        {

        }

        public ConnectorInfoBase(IDiagramViewModel root, SerializableItem serializableItem, string serializableType) : base(root, serializableItem, serializableType)
        {

        }

        public override SelectableItemBase GetSerializableObject()
        {
            return new ConnectorInfoItemBase(this);
        }

        protected override void Init(IDiagramViewModel root, bool initNew)
        {
            base.Init(root, initNew);
        }

        protected override void InitNew()
        {
            ColorViewModel = new ColorViewModel()
            {
                LineColor = new ColorObject() { Color = Color.FromArgb(0xAA, 0x00, 0x00, 0x80) },
                FillColor = new ColorObject() { Color = Colors.Lavender },
            };
        }

        protected override void LoadDesignerItemViewModel(SelectableItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(designerbase);

            if (designerbase is ConnectorInfoItemBase designer)
            {
                PhysicalConnectorWidth = designer.PhysicalConnectorWidth;
                PhysicalConnectorHeight = designer.PhysicalConnectorHeight;
                Orientation = designer.Orientation;
            }
        }

        #region 属性
        public virtual PointBase Position
        {
            get; set;
        }

        public virtual PointBase MiddlePosition
        {
            get
            {
                return new PointBase(Position.X + ConnectorWidth / 2, Position.Y + ConnectorHeight / 2);
            }
        }

        private ConnectorOrientation _orientation;
        public ConnectorOrientation Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                SetProperty(ref _orientation, value);
            }
        }

        private double _connectorWidth = 8;
        public double ConnectorWidth
        {
            get
            {
                return _connectorWidth;
            }
            set
            {
                if (SetProperty(ref _connectorWidth, value))
                {
                    RaisePropertyChanged(nameof(PhysicalConnectorWidth));
                }
            }
        }

        private double _connectorHeight = 8;
        public double ConnectorHeight
        {
            get
            {
                return _connectorHeight;
            }
            set
            {
                if (SetProperty(ref _connectorHeight, value))
                {
                    RaisePropertyChanged(nameof(PhysicalConnectorHeight));
                }
            }
        }

        public double PhysicalConnectorWidth
        {
            get
            {
                return ScreenHelper.WidthToMm(ConnectorWidth);
            }
            set
            {
                ConnectorWidth = ScreenHelper.MmToWidth(value);
            }
        }

        public double PhysicalConnectorHeight
        {
            get
            {
                return ScreenHelper.WidthToMm(ConnectorHeight);
            }
            set
            {
                ConnectorHeight = ScreenHelper.MmToWidth(value);
            }
        }

        private bool _beAttachTo;
        public bool BeAttachTo
        {
            get
            {
                return _beAttachTo;
            }
            set
            {
                SetProperty(ref _beAttachTo, value);
            }
        }

        private bool _disableAttachTo;
        public bool DisableAttachTo
        {
            get
            {
                return _disableAttachTo;
            }
            set
            {
                SetProperty(ref _disableAttachTo, value);
            }
        }

        public virtual bool CanAttachTo(ConnectorInfoBase port)
            => port != null && port != this && !port.IsReadOnly;
    }
}
