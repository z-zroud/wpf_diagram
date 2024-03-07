using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfDiagram.Enums
{
    public enum LineAnimation
    {
        [Description("无")]
        None,
        [Description("路径动画")]
        PathAnimation,
        [Description("虚线流动")]
        DashAnimation,
    }
}
