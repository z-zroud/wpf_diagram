using System;
using AIStudio.Wpf.DiagramDesigner.Geometrys;

namespace WpfDiagram.Helper
{
    public class PointHelper
    {
        public static PointBase GetPointForConnector(FullyCreatedConnectorInfo connector, bool middle = false)
        {
            PointBase point = new PointBase();
            if (connector == null || connector.DataItem == null)
            {
                return point;
            }

            var connectorWidth =  double.IsNaN(connector.ConnectorWidth) ? 0 : connector.ConnectorWidth;
            var connectorHeight = double.IsNaN(connector.ConnectorHeight) ? 0 : connector.ConnectorHeight;
            var left = connector.DataItem.Left;
            var top = connector.DataItem.Top;
            var itemWidth = connector.DataItem.GetItemWidth();
            var itemHeight = connector.DataItem.GetItemHeight();

            if (connector.IsInnerPoint)
            {
                point = new PointBase(left + itemWidth * connector.XRatio - connectorWidth / 2,
                        top + itemHeight * connector.YRatio - connectorHeight / 2);
            }
            else if (connector.IsPortless)
            {
                point = connector.DataItem.MiddlePosition;
            }
            else
            {
                switch (connector.Orientation)
                {
                    case ConnectorOrientation.Left:
                        point = new PointBase(left - connectorWidth / 2, top + (itemHeight / 2) - connectorHeight / 2);
                        break;
                    case ConnectorOrientation.TopLeft:
                        point = new PointBase(left - connectorWidth / 2, top - connectorHeight / 2);
                        break;
                    case ConnectorOrientation.Top:
                        point = new PointBase(left + (itemWidth / 2) - connectorWidth / 2, top - connectorHeight / 2);
                        break;
                    case ConnectorOrientation.TopRight:
                        point = new PointBase(left + itemWidth - connectorWidth / 2, top - connectorHeight / 2);
                        break;
                    case ConnectorOrientation.Right:
                        point = new PointBase(left + itemWidth - connectorWidth / 2, top + (itemHeight / 2) - connectorHeight / 2);
                        break;
                    case ConnectorOrientation.BottomRight:
                        point = new PointBase(left + itemWidth - connectorWidth / 2, top + itemHeight - connectorHeight / 2);
                        break;
                    case ConnectorOrientation.Bottom:
                        point = new PointBase(left + (itemWidth / 2) - connectorWidth / 2, top + itemHeight - connectorHeight / 2);
                        break;
                    case ConnectorOrientation.BottomLeft:
                        point = new PointBase(left - connectorWidth / 2, top + itemHeight - connectorHeight / 2);
                        break;
                    default:
                        point = new PointBase(left + (itemWidth / 2) - connectorWidth / 2, top + (itemHeight / 2) - connectorHeight / 2);
                        break;
                }
            }

            if (middle)
            {
                point.X = point.X + connectorWidth / 2;
                point.Y = point.Y + connectorHeight / 2;
            }
            //旋转后的坐标
            var newX = (point.X - connector.DataItem.MiddlePosition.X) * Math.Cos(connector.DataItem.Angle * Math.PI / 180) - (point.Y - connector.DataItem.MiddlePosition.Y) * Math.Sin(connector.DataItem.Angle * Math.PI / 180) + connector.DataItem.MiddlePosition.X;
            var newY = (point.Y - connector.DataItem.MiddlePosition.Y) * Math.Cos(connector.DataItem.Angle * Math.PI / 180) - (point.X - connector.DataItem.MiddlePosition.X) * Math.Sin(connector.DataItem.Angle * Math.PI / 180) + connector.DataItem.MiddlePosition.Y;
            //放大缩小后的坐标

            newX = (newX - connector.DataItem.MiddlePosition.X) * connector.DataItem.ScaleX + connector.DataItem.MiddlePosition.X;
            newY = (newY - connector.DataItem.MiddlePosition.Y) * connector.DataItem.ScaleY + connector.DataItem.MiddlePosition.Y;
            return new PointBase(newX, newY);
        }
    }
}
