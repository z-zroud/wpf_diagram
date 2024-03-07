using System;
using System.Collections.Generic;
using System.Text;

namespace WpfDiagram.Enums
{
    public enum DrawMode
    {
        Normal = 0,       
        ConnectingLineSmooth = 10,
        ConnectingLineStraight = 11,
        ConnectingLineCorner = 12,
        ConnectingLineBoundary = 13,
        //实心图使用
        Text = 119,
        Line = 120,
        Rectangle = 121,
        Ellipse = 122,
        Polyline = 123,
        Polygon = 124,
        DirectLine = 125,
        //200-300为可部分擦除的形状
        Select = 200,
        Eraser = 201,
        EraserPreview = 202,
        //画笔使用
        Pen1 = 210,
        Pen2 = 211,
        Pen3 = 212,
        ColorPicker = 213,
        ErasableText = 219,
        ErasableLine = 220,
        ErasableRectangle = 221,
        ErasableEllipse = 222,
        ErasablePolyline = 223,
        ErasablePolygon = 224,
        ErasableDirectLine = 225,
        ErasableTriangle = 226, //三角形  
        ErasableRhombus = 227, //菱形        
        ErasableHexagon = 228,//六边形
        ErasablePentagram = 229,//五角星
        ErasableStarFour = 230,//四角星
        ErasableStarThree = 231,//三角星
        ErasableChat = 232,//对话框
        ErasableComment = 233,//评论
        ErasableCloud = 234,//云
        ErasableArrowRight = 235,
        ErasableArrowLeft = 236,
        ErasableCheck = 237,
        ErasableClose = 238,
        ErasableHeart = 239,
    }
}
