using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfDiagram.Attributes;
using WpfDiagram.Common;
using WpfDiagram.Enums;
using WpfDiagram.Helper;
using WpfDiagram.Interface;
using WpfDiagram.Models.Serializables;

namespace WpfDiagram.ViewModels
{
    public abstract class SelectableViewModelBase : BindableBase, ISelectable, IDisposable
    {
        protected IDiagramServiceProvider _service
        {
            get
            {
                return DiagramServicesProvider.Instance.Provider;
            }
        }

        public SelectableViewModelBase() : this(null)
        {

        }

        public SelectableViewModelBase(IDiagramViewModel root)
        {
            IsLoaded = false;
            Init(root, true);
            IsLoaded = true;
        }

        public SelectableViewModelBase(IDiagramViewModel root, SelectableItemBase designer)
        {
            IsLoaded = false;
            Init(root, false);
            LoadDesignerItemViewModel(designer);
            IsLoaded = true;
        }

        public SelectableViewModelBase(IDiagramViewModel root, SerializableItem serializableItem, string serializableType = null)
        {
            IsLoaded = false;
            Init(root, false);
            SelectableItemBase obj = SerializeHelper.DeserializeObject(serializableItem.SerializableTypeName, serializableItem.SerializableString, serializableType);
            LoadDesignerItemViewModel(obj);
            IsLoaded = true;
        }

        public virtual SerializableItem ToSerializableItem(string serializableType = null)
        {
            var obj = GetSerializableObject();
            if (obj != null)
            {
                return new SerializableItem() { ModelTypeName = this.GetType().FullName, SerializableTypeName = obj.GetType().FullName, SerializableString = SerializeHelper.SerializeObject(obj, serializableType) };
            }
            else
            {
                return null;
            }
        }

        public virtual SelectableItemBase GetSerializableObject()
        {
            return null;
        }

        protected virtual void Init(IDiagramViewModel root, bool initNew)
        {
            Root = root;

            if (Root?.ColorViewModel != null)
            {
                this.ColorViewModel = CopyHelper.Mapper(Root.ColorViewModel);
            }
            else
            {
                this.ColorViewModel = _service.CopyDefaultColorViewModel();
            }

            if (Root?.FontViewModel != null)
            {
                this.FontViewModel = CopyHelper.Mapper(Root.FontViewModel);
            }
            else
            {
                this.FontViewModel = _service.CopyDefaultFontViewModel();
            }

            if (Root?.ShapeViewModel != null)
            {
                this.ShapeViewModel = CopyHelper.Mapper(Root.ShapeViewModel);
            }
            else
            {
                this.ShapeViewModel = _service.CopyDefaultShapeViewModel();
            }

            if (Root?.AnimationViewModel != null)
            {
                this.AnimationViewModel = CopyHelper.Mapper(Root.AnimationViewModel);
            }
            else
            {
                this.AnimationViewModel = _service.CopyDefaultAnimationViewModel();
            }

            LockObjectViewModel = new LockObjectViewModel();

            if (initNew)
            {
                InitNew();
            }

            this.PropertyChanged -= Item_PropertyChanged;
            this.PropertyChanged += Item_PropertyChanged;
        }

        protected virtual void InitNew()
        {

        }

        protected virtual void LoadDesignerItemViewModel(SelectableItemBase designerbase)
        {
            this.Id = designerbase.Id;
            this.ParentId = designerbase.ParentId;
            this.IsGroup = designerbase.IsGroup;
            this.ZIndex = designerbase.ZIndex;
            this.Text = designerbase.Text;
            this.Name = designerbase.Name;

            ColorViewModel = CopyHelper.Mapper(designerbase.ColorItem);
            FontViewModel = CopyHelper.Mapper<FontViewModel, FontItem>(designerbase.FontItem);
            ShapeViewModel = CopyHelper.Mapper(designerbase.SharpItem);
            AnimationViewModel = CopyHelper.Mapper(designerbase.AnimationItem);
        }

        public object Tag
        {
            get; set;
        }

        public bool IsLoaded
        {
            get; set;
        }

        public bool IsInternalChanged
        {
            get; set;
        }

        public IDiagramViewModel Root
        {
            get; set;
        }

        public SelectableViewModelBase Parent
        {
            get; set;
        }

        public Guid Id
        {
            get; set;
        }

        private Guid _parentId;
        public Guid ParentId
        {
            get
            {
                return _parentId;
            }
            set
            {
                SetProperty(ref _parentId, value);
            }
        }

        public bool IsGroup
        {
            get; set;
        }

        private bool _disableSelected;
        public bool SelectedDisable
        {
            get
            {
                return _disableSelected;
            }
            set
            {
                SetProperty(ref _disableSelected, value);
            }
        }

        private bool _isSelected;
        [Browsable(false)]
        //[CanDo]
        public bool IsSelected
        {
            get
            {
                if (SelectedDisable == true)
                {
                    return false;
                }
                return _isSelected;
            }
            set
            {
                if (SetProperty(ref _isSelected, value))
                {
                    //如果没有文字，失去焦点自动清除
                    if (_isSelected == false && string.IsNullOrEmpty(Text))
                    {
                        ClearText();
                    }
                }
            }
        }

        public bool InitIsEditing
        {
            get; set;
        }

        private bool _isEditing = false;
        public bool IsEditing
        {
            get
            {
                return _isEditing;
            }
            set
            {
                SetProperty(ref _isEditing, value);
            }
        }

        private bool _showText;
        public bool ShowText
        {
            get
            {
                return _showText;
            }
            set
            {
                if (!SetProperty(ref _showText, value))
                {
                    RaisePropertyChanged(nameof(ShowText));
                }
            }
        }

        private bool _visible = true;
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                SetProperty(ref _visible, value);
            }
        }

        private int _zIndex;
        [Browsable(true)]
        [CanDo]
        public int ZIndex
        {
            get
            {
                return _zIndex;
            }
            set
            {
                if (value == int.MaxValue)
                {

                }
                SetProperty(ref _zIndex, value);
            }
        }

        private bool _isReadOnly;
        [Browsable(false)]
        public bool IsReadOnly
        {
            get
            {
                if (Root?.IsReadOnly == true && Root?.IsLoading == false)
                {
                    return true;
                }

                if (Parent?.IsReadOnly == true)
                {
                    return true;
                }

                if (LockObjectViewModel?.LockObject.FirstOrDefault(p => p.LockFlag == LockFlag.All)?.IsChecked == true)
                {
                    return true;
                }
                return _isReadOnly;
            }
            set
            {
                SetProperty(ref _isReadOnly, value);
            }
        }

        private bool _isHitTestVisible = true;
        [Browsable(false)]
        public bool IsHitTestVisible
        {
            get
            {
                if (Parent?.IsHitTestVisible == false)
                {
                    return false;
                }

                return _isHitTestVisible;
            }
            set
            {
                if (SetProperty(ref _isHitTestVisible, value))
                {
                    RaisePropertyChanged("IsReadOnly");
                }
            }
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
                if (_colorViewModel != null)
                {
                    _colorViewModel.PropertyChanged -= ColorViewModel_PropertyChanged;
                }
                SetProperty(ref _colorViewModel, value);
                if (_colorViewModel != null)
                {
                    _colorViewModel.PropertyChanged += ColorViewModel_PropertyChanged;
                }
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
                if (_fontViewModel != null)
                {
                    _fontViewModel.PropertyChanged -= FontViewModel_PropertyChanged;
                }
                SetProperty(ref _fontViewModel, value);
                if (_fontViewModel != null)
                {
                    _fontViewModel.PropertyChanged += FontViewModel_PropertyChanged;
                }
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
                if (_shapeViewModel != null)
                {
                    _shapeViewModel.PropertyChanged -= ShapeViewModel_PropertyChanged;
                }
                SetProperty(ref _shapeViewModel, value);
                if (_shapeViewModel != null)
                {
                    _shapeViewModel.PropertyChanged += ShapeViewModel_PropertyChanged;
                }
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
                if (_animationViewModel != null)
                {
                    _animationViewModel.PropertyChanged -= AnimationViewModel_PropertyChanged;
                }
                SetProperty(ref _animationViewModel, value);
                if (_animationViewModel != null)
                {
                    _animationViewModel.PropertyChanged += AnimationViewModel_PropertyChanged;
                }
            }
        }

        public ILockObjectViewModel LockObjectViewModel
        {
            get; set;
        }

        [Browsable(true)]
        public string Name
        {
            get; set;
        }

        private string _text;
        [Browsable(true)]
        [CanDo]
        public virtual string Text
        {
            get
            {
                var text = _text;
                if (FontViewModel.FontCase == FontCase.Upper)
                {
                    return text?.ToUpper();
                }
                else if (FontViewModel.FontCase == FontCase.Lower)
                {
                    return text?.ToLower();
                }
                else
                {
                    return text;
                }
            }
            set
            {
                if (SetProperty(ref _text, value))
                {

                }
            }
        }

        protected virtual void ClearText()
        {

        }

        public virtual void RemoveFromSelection()
        {
            IsSelected = false;
        }

        public virtual void AddToSelection(bool selected, bool clearother)
        {

        }

        protected bool Command_Enable(object para)
        {
            return IsReadOnly == false;
        }

        protected virtual void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsLoaded == false || IsInternalChanged == true) { return; }
        }

        protected void FontViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsLoaded == false || IsInternalChanged == true) { return; }

            if (e.PropertyName == nameof(FontViewModel.FontCase))
            {
                RaisePropertyChanged("Text");
            }

            RaisePropertyChanged(sender, e);
        }

        protected void ColorViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsLoaded == false || IsInternalChanged == true) { return; }

            RaisePropertyChanged(sender, e);
        }

        protected void ShapeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsLoaded == false || IsInternalChanged == true) { return; }

            RaisePropertyChanged(sender, e);
        }

        protected void AnimationViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsLoaded == false || IsInternalChanged == true) { return; }

            RaisePropertyChanged(sender, e);
        }


        public override string ToString()
        {
            return $"{Id}-{Name}-{Text}";
        }

        public virtual void Dispose()
        {
        }
    }
}
