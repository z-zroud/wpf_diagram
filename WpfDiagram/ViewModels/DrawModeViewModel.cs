using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using WpfDiagram.Common;
using WpfDiagram.Interface;

namespace WpfDiagram.ViewModels
{
    public class DrawModeViewModel : BindableBase, IDrawModeViewModel
    {
        public DrawModeViewModel()
        {
            DrawingColorViewModel1 = new ColorViewModel(Color.FromRgb(0x19, 0x19, 0x1a), Colors.Transparent) { LineWidth = 1 };
            DrawingColorViewModel2 = new ColorViewModel(Color.FromRgb(0xf5, 0xdc, 0x4e), Colors.Transparent) { LineWidth = 4 };
            DrawingColorViewModel3 = new ColorViewModel(Color.FromRgb(0xf2, 0x69, 0x57), Colors.Transparent) { LineWidth = 8 };
        }

        public DrawMode GetDrawMode()
        {  
            if (DrawingDrawModeSelected)
            {
                return DrawingDrawMode;
            }
            else if (CursorDrawModeSelected)
            {
                return CursorDrawMode;
            }
            else if (LineDrawModeSelected)
            {
                return LineDrawMode;
            }
            else if (SharpDrawModeSelected)
            {
                return SharpDrawMode;
            }
            else if (TextDrawModeSelected)
            {
                return TextDrawMode;
            }

            return DrawMode.Normal;
        }

        public void ResetDrawMode()
        {
            var drawingDrawMode = GetOldValue<DrawMode>(nameof(DrawingDrawMode));
            if (drawingDrawMode != default(DrawMode))
            {
                ClearOldValue<DrawMode>(nameof(DrawingDrawMode));
                DrawingDrawMode = drawingDrawMode;
            }
            else
            {
                CursorDrawModeSelected = true;
                CursorDrawMode = DrawMode.Normal;
            }
        }

        public void SetDrawMode(DrawMode drawMode)
        {
            CursorDrawMode = drawMode;
        }

        private bool _cursordrawModeSelected = true;
        public bool CursorDrawModeSelected
        {
            get
            {
                return _cursordrawModeSelected;
            }
            set
            {
                SetProperty(ref _cursordrawModeSelected, value);
            }
        }

        private DrawMode _cursordrawMode = DrawMode.Normal;
        public DrawMode CursorDrawMode
        {
            get
            {
                return _cursordrawMode;
            }
            set
            {
                SetProperty(ref _cursordrawMode, value);
                CursorDrawModeSelected = true;
            }
        }

        private bool _lineDrawModeSelected;
        public bool LineDrawModeSelected
        {
            get
            {
                return _lineDrawModeSelected;
            }
            set
            {
                SetProperty(ref _lineDrawModeSelected, value);
            }
        }

        private DrawMode _lineDrawMode = DrawMode.ConnectingLineSmooth;
        public DrawMode LineDrawMode
        {
            get
            {
                return _lineDrawMode;
            }
            set
            {
                SetProperty(ref _lineDrawMode, value);
                LineDrawModeSelected = true;
            }
        }

        #region 界面还未使用到
        private bool _vectorRouterModeSelected;
        public bool LineRouterModeSelected
        {
            get
            {
                return _vectorRouterModeSelected;
            }
            set
            {
                SetProperty(ref _vectorRouterModeSelected, value);
            }
        }


        private RouterMode _lineRouterMode = RouterMode.RouterNormal;
        public RouterMode LineRouterMode
        {
            get
            {
                return _lineRouterMode;
            }
            set
            {
                SetProperty(ref _lineRouterMode, value);
            }
        }
        #endregion

        private bool _shapeDrawModeSelected;
        public bool SharpDrawModeSelected
        {
            get
            {
                return _shapeDrawModeSelected;
            }
            set
            {
                SetProperty(ref _shapeDrawModeSelected, value);
            }
        }

        private DrawMode _sharpDrawMode = DrawMode.Rectangle;
        public DrawMode SharpDrawMode
        {
            get
            {
                return _sharpDrawMode;
            }
            set
            {
                SetProperty(ref _sharpDrawMode, value);
                SharpDrawModeSelected = true;
            }
        }

        private bool _drawingDrawModeSelected;
        public bool DrawingDrawModeSelected
        {
            get
            {
                return _drawingDrawModeSelected;
            }
            set
            {
                SetProperty(ref _drawingDrawModeSelected, value);
            }
        }

        private DrawMode _drawingDrawMode = DrawMode.ErasableLine;
        public DrawMode DrawingDrawMode
        {
            get
            {
                return _drawingDrawMode;
            }
            set
            {
                if (value == DrawMode.ColorPicker)
                {
                    SetOldValue(_drawingDrawMode, nameof(DrawingDrawMode));
                }
                SetProperty(ref _drawingDrawMode, value);
                DrawingDrawModeSelected = true;
             
            }
        }

        private bool _textDrawModeSelected;
        public bool TextDrawModeSelected
        {
            get
            {
                return _textDrawModeSelected;
            }
            set
            {
                SetProperty(ref _textDrawModeSelected, value);
            }
        }

        private DrawMode _textDrawMode = DrawMode.Text;
        public DrawMode TextDrawMode
        {
            get
            {
                return _textDrawMode;
            }
            set
            {
                SetProperty(ref _textDrawMode, value);
                TextDrawModeSelected = true;
            }
        }

        private CursorMode _cursorMode;
        public CursorMode CursorMode
        {
            get
            {
                return _cursorMode;
            }
            set
            {
                SetProperty(ref _cursorMode, value);
            }
        }

        #region 画笔使用
        private DrawMode _drawingPenDrawMode = DrawMode.Pen1;
        public DrawMode DrawingPenDrawMode
        {
            get
            {
                return _drawingPenDrawMode;
            }
            set
            {
                SetProperty(ref _drawingPenDrawMode, value);
            }
        }

        public IColorViewModel DrawingColorViewModel
        {
            get
            {
                if (DrawingPenDrawMode == DrawMode.Pen1)
                {
                    return DrawingColorViewModel1;
                }
                else if (DrawingPenDrawMode == DrawMode.Pen2)
                {
                    return DrawingColorViewModel2;
                }
                else if (DrawingPenDrawMode == DrawMode.Pen3)
                {
                    return DrawingColorViewModel3;
                }
                else
                {
                    return DrawingColorViewModel1;
                }
            }
        }

        private IColorViewModel _drawingColorViewModel1;
        public IColorViewModel DrawingColorViewModel1
        {
            get
            {
                return _drawingColorViewModel1;
            }
            set
            {
                SetProperty(ref _drawingColorViewModel1, value);
            }
        }

        private IColorViewModel _drawingColorViewModel2;
        public IColorViewModel DrawingColorViewModel2
        {
            get
            {
                return _drawingColorViewModel2;
            }
            set
            {
                SetProperty(ref _drawingColorViewModel2, value);
            }
        }

        private IColorViewModel _drawingColorViewModel3;
        public IColorViewModel DrawingColorViewModel3
        {
            get
            {
                return _drawingColorViewModel3;
            }
            set
            {
                SetProperty(ref _drawingColorViewModel3, value);
            }
        }
        #endregion
    }
}
