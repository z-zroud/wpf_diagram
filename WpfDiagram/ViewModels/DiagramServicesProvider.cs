using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfDiagram.Common;
using WpfDiagram.Helper;
using WpfDiagram.Interface;

namespace WpfDiagram.ViewModels
{
    public class DiagramServiceProvider : BindableBase, IDiagramServiceProvider
    {
        public DiagramServiceProvider()
        {
            ColorViewModel = new ColorViewModel();
            FontViewModel = new FontViewModel();
            ShapeViewModel = new ShapeViewModel();
            AnimationViewModel = new AnimationViewModel();
            LockObjectViewModel = new LockObjectViewModel();

            _drawModeViewModel = new DrawModeViewModel();
            _quickThemeViewModel = new QuickThemeViewModel();

            _drawModeViewModel.PropertyChanged += ViewModel_PropertyChanged;
            _quickThemeViewModel.PropertyChanged += ViewModel_PropertyChanged;

            SetOldValue(ColorViewModel, nameof(ColorViewModel));
            SetOldValue(FontViewModel, nameof(FontViewModel));
            SetOldValue(ShapeViewModel, nameof(ShapeViewModel));
            SetOldValue(AnimationViewModel, nameof(AnimationViewModel));
            SetOldValue(LockObjectViewModel, nameof(LockObjectViewModel));
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(sender, e.PropertyName);
        }

        public IColorViewModel CopyDefaultColorViewModel()
        {
            var viewModel = GetOldValue<ColorViewModel>(nameof(ColorViewModel));
            return CopyHelper.Mapper(viewModel);
        }

        public IFontViewModel CopyDefaultFontViewModel()
        {
            var viewModel = GetOldValue<FontViewModel>(nameof(FontViewModel));
            return CopyHelper.Mapper(viewModel);
        }

        public IShapeViewModel CopyDefaultShapeViewModel()
        {
            var viewModel = GetOldValue<ShapeViewModel>(nameof(ShapeViewModel));
            return CopyHelper.Mapper(viewModel);
        }

        public IAnimationViewModel CopyDefaultAnimationViewModel()
        {
            var viewModel = GetOldValue<AnimationViewModel>(nameof(AnimationViewModel));
            return CopyHelper.Mapper(viewModel);
        }

        private IColorViewModel _colorViewModel;
        public IColorViewModel ColorViewModel
        {
            get
            {
                return _colorViewModel;
            }
            set
            {
                SetProperty(ref _colorViewModel, value);
            }
        }



        private IFontViewModel _fontViewModel;
        public IFontViewModel FontViewModel
        {
            get
            {
                return _fontViewModel;
            }
            set
            {
                SetProperty(ref _fontViewModel, value);
            }
        }

        private IShapeViewModel _shapeViewModel;
        public IShapeViewModel ShapeViewModel
        {
            get
            {
                return _shapeViewModel;
            }
            set
            {
                SetProperty(ref _shapeViewModel, value);
            }
        }

        private IAnimationViewModel _animationViewModel;
        public IAnimationViewModel AnimationViewModel
        {
            get
            {
                return _animationViewModel;
            }
            set
            {
                SetProperty(ref _animationViewModel, value);
            }
        }

        private IDrawModeViewModel _drawModeViewModel;
        public IDrawModeViewModel DrawModeViewModel
        {
            get
            {
                return _drawModeViewModel;
            }
        }

        private IQuickThemeViewModel _quickThemeViewModel;
        public IQuickThemeViewModel QuickThemeViewModel
        {
            get
            {
                return _quickThemeViewModel;
            }
        }

        private ILockObjectViewModel _lockObjectViewModel;
        public ILockObjectViewModel LockObjectViewModel
        {
            get
            {
                return _lockObjectViewModel;
            }
            set
            {
                SetProperty(ref _lockObjectViewModel, value);
            }
        }


        public List<SelectableDesignerItemViewModelBase> SelectedItems
        {
            get; set;
        }

        private SelectableDesignerItemViewModelBase _selectedItemViewModel;
        public SelectableDesignerItemViewModelBase SelectedItemViewModel
        {
            get
            {
                return _selectedItemViewModel;
            }
            set
            {
                if (_selectedItemViewModel != null)
                {
                    _selectedItemViewModel.PropertyChanged -= ViewModel_PropertyChanged;
                }
                if (SetProperty(ref _selectedItemViewModel, value))
                {
                    if (_selectedItemViewModel == null)
                    {
                        ColorViewModel = GetOldValue<ColorViewModel>(nameof(ColorViewModel));
                        FontViewModel = GetOldValue<FontViewModel>(nameof(FontViewModel));
                        ShapeViewModel = GetOldValue<ShapeViewModel>(nameof(ShapeViewModel));
                        AnimationViewModel = GetOldValue<AnimationViewModel>(nameof(AnimationViewModel));
                        LockObjectViewModel = GetOldValue<LockObjectViewModel>(nameof(LockObjectViewModel));
                    }
                    else
                    {
                        ColorViewModel = _selectedItemViewModel.ColorViewModel;
                        FontViewModel = _selectedItemViewModel.FontViewModel;
                        ShapeViewModel = _selectedItemViewModel.ShapeViewModel;
                        AnimationViewModel = _selectedItemViewModel.AnimationViewModel;
                        LockObjectViewModel = _selectedItemViewModel.LockObjectViewModel;
                    }
                }
                if (_selectedItemViewModel != null)
                {
                    _selectedItemViewModel.PropertyChanged += ViewModel_PropertyChanged;
                }
            }
        }
    }

    /// <summary>
    /// Simple service locator helper
    /// </summary>
    public class DiagramServicesProvider
    {
        private static Lazy<DiagramServicesProvider> instance = new Lazy<DiagramServicesProvider>(() => new DiagramServicesProvider());
        private IDiagramServiceProvider serviceProvider = new DiagramServiceProvider();

        public void SetNewServiceProvider(IDiagramServiceProvider provider)
        {
            serviceProvider = provider;
        }

        public IDiagramServiceProvider Provider
        {
            get
            {
                return serviceProvider;
            }
        }

        public static DiagramServicesProvider Instance
        {
            get
            {
                return instance.Value;
            }
        }
    }
}
