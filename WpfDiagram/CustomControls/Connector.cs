using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfDiagram.Enums;

namespace WpfDiagram.CustomControls
{
    public class Connector : ContentControl
    {
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DesignerCanvas canvas = GetDesignerCanvas(this);
            if (canvas != null)
            {
                canvas.SourceConnector = this;
            }
        }

        public ConnectorOrientation Orientation { get; set; }

        // iterate through visual tree to get parent DesignerCanvas
        private DesignerCanvas GetDesignerCanvas(DependencyObject element)
        {
            while (element != null && !(element is DesignerCanvas))
                element = VisualTreeHelper.GetParent(element);

            return element as DesignerCanvas;
        }

        public ConnectorInfoBase Info
        {
            get
            {
                if (Content is ConnectorInfoBase connectorInfo)
                    return connectorInfo;

                return this.DataContext as ConnectorInfoBase;
            }
        }
    }
}
