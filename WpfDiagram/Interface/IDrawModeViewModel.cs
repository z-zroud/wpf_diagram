using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfDiagram.Interface
{
    public interface IDrawModeViewModel
    {
        DrawMode GetDrawMode();
        void SetDrawMode(DrawMode drawMode);

        void ResetDrawMode();

        bool CursorDrawModeSelected
        {
            get; set;
        }
        CursorMode CursorMode
        {
            get; set;
        }
        bool LineDrawModeSelected
        {
            get; set;
        }
        DrawMode LineDrawMode
        {
            get; set;
        }
        bool TextDrawModeSelected
        {
            get; set;
        }
        DrawMode TextDrawMode
        {
            get; set;
        }
        //界面还未使用到
        RouterMode LineRouterMode
        {
            get; set;
        }
        bool SharpDrawModeSelected
        {
            get; set;
        }
        DrawMode SharpDrawMode
        {
            get; set;
        }
        bool DrawingDrawModeSelected
        {
            get; set;
        }
        DrawMode DrawingDrawMode
        {
            get; set;
        }
        IColorViewModel DrawingColorViewModel
        {
            get;
        }

        event PropertyChangedEventHandler PropertyChanged;
    }
}
