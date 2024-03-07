using System;
using System.Collections.Generic;
using System.Text;

namespace WpfDiagram.Enums
{
    public enum ConnectorValueType
    {
        //前面分给值类型，值类型之间可以兼容，直接转换
        Real = 0,
        Int = 1,
        Bool = 2,
        ValueType = 99,
        String = 100,
        JsonString = 101,
    }
}
