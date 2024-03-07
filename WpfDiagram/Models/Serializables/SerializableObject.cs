using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WpfDiagram.Enums;
using WpfDiagram.Helper;
using WpfDiagram.ViewModels;

namespace WpfDiagram.Models.Serializables
{
    [Serializable]
    [XmlInclude(typeof(SerializableItem))]
    public class SerializableItem
    {
        [XmlIgnore]
        public Type ModelType
        {
            get; set;
        }

        [XmlAttribute]
        public string ModelTypeName
        {
            get; set;
        }

        [XmlIgnore]
        public Type SerializableType
        {
            get; set;
        }

        [XmlAttribute]
        public string SerializableTypeName
        {
            get; set;
        }

        [XmlAttribute]
        public string SerializableString
        {
            get; set;
        }
    }

    [Serializable]
    [XmlInclude(typeof(SerializableObject))]
    public class SerializableObject
    {
        [XmlArray]
        public List<SerializableItem> DesignerItems { get; set; } = new List<SerializableItem>();

        [XmlArray]
        public List<SerializableItem> Connections { get; set; } = new List<SerializableItem>();

        public List<SelectableDesignerItemViewModelBase> ToObject()
        {
            List<DesignerItemViewModelBase> items = new List<DesignerItemViewModelBase>();
            foreach (var diagramItemData in this.DesignerItems)
            {
                Type itemtype = TypeHelper.GetType(diagramItemData.ModelTypeName);
                DesignerItemViewModelBase itemBase = Activator.CreateInstance(itemtype, null, diagramItemData, ".json") as DesignerItemViewModelBase;
                items.Add(itemBase);
            }
            List<ConnectionViewModel> connects = new List<ConnectionViewModel>();
            foreach (var connection in this.Connections)
            {
                Type itemtype = TypeHelper.GetType(connection.SerializableTypeName);
                var connectionItem = SerializeHelper.DeserializeObject(itemtype, connection.SerializableString, ".json") as ConnectionItem;

                connectionItem.SourceType = TypeHelper.GetType(connectionItem.SourceTypeName);
                connectionItem.SinkType = TypeHelper.GetType(connectionItem.SinkTypeName);
                DesignerItemViewModelBase sourceItem = DiagramViewModelHelper.GetConnectorDataItem(items, connectionItem.SourceId, connectionItem.SourceType);
                if (sourceItem == null)
                    continue;
                ConnectorOrientation sourceConnectorOrientation = connectionItem.SourceOrientation;
                FullyCreatedConnectorInfo sourceConnectorInfo = sourceItem.GetFullConnectorInfo(connectionItem.Id, sourceConnectorOrientation, connectionItem.SourceXRatio, connectionItem.SourceYRatio, connectionItem.SourceInnerPoint, connectionItem.SourceInnerPoint);

                DesignerItemViewModelBase sinkItem = DiagramViewModelHelper.GetConnectorDataItem(items, connectionItem.SinkId, connectionItem.SinkType);
                if (sinkItem == null)
                    continue;
                ConnectorOrientation sinkConnectorOrientation = connectionItem.SinkOrientation;
                FullyCreatedConnectorInfo sinkConnectorInfo = sinkItem.GetFullConnectorInfo(connectionItem.Id, sinkConnectorOrientation, connectionItem.SinkXRatio, connectionItem.SinkYRatio, connectionItem.SinkInnerPoint, connectionItem.SinkInnerPoint);

                ConnectionViewModel connectionVM = new ConnectionViewModel(null, sourceConnectorInfo, sinkConnectorInfo, connectionItem);
                connectionVM.Id = Guid.NewGuid();
                connects.Add(connectionVM);
            }

            var viewmodels = new List<SelectableDesignerItemViewModelBase>();
            viewmodels.AddRange(items);
            viewmodels.AddRange(connects);

            return viewmodels;
        }
    }
}
