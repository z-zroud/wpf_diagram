using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfDiagram.Common;
using WpfDiagram.Interface;
using WpfDiagram.Models.Serializables;

namespace WpfDiagram.ViewModels
{
    public interface ISelectItems
    {
        ICommand SelectItemCommand
        {
            get;
        }
    }

    public class SelectableDesignerItemViewModelBase : SelectableViewModelBase, ISelectItems, ISelectable, IGroupable
    {
        public SelectableDesignerItemViewModelBase() : this(null)
        {

        }

        public SelectableDesignerItemViewModelBase(IDiagramViewModel root) : base(root)
        {

        }

        public SelectableDesignerItemViewModelBase(IDiagramViewModel root, SelectableItemBase designer) : base(root, designer)
        {

        }

        public SelectableDesignerItemViewModelBase(IDiagramViewModel root, SerializableItem serializableItem, string serializableType) : base(root, serializableItem, serializableType)
        {

        }

        protected override void Init(IDiagramViewModel root, bool initNew)
        {
            base.Init(root, initNew);
        }

        protected override void InitNew()
        {
            base.InitNew();
        }

        protected override void LoadDesignerItemViewModel(SelectableItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(designerbase);
        }

        public virtual bool Verify()
        {
            return true;
        }
        public virtual bool EditData()
        {
            return true;
        }

        private ICommand _selectItemCommand;
        public ICommand SelectItemCommand
        {
            get
            {
                return this._selectItemCommand ?? (this._selectItemCommand = new SimpleCommand(Command_Enable, ExecuteSelectItemCommand));
            }
        }

        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                return this._editCommand ?? (this._editCommand = new SimpleCommand(Command_Enable, ExecuteEditCommand));
            }
        }

        private ICommand _exitEditCommand;
        public ICommand ExitEditCommand
        {
            get
            {
                return this._exitEditCommand ?? (this._exitEditCommand = new SimpleCommand(Command_Enable, ExecuteExitEditCommand));
            }
        }

        public int EditClickCount
        {
            get; set;
        } = 2;

        private bool enabledForSelection = true;
        public bool EnabledForSelection
        {
            get
            {
                return enabledForSelection;
            }
            set
            {
                SetProperty(ref enabledForSelection, value);
            }
        }


        private bool _isReadOnlyText = false;
        public bool IsReadOnlyText
        {
            get
            {
                if (IsReadOnly)
                    return true;
                return _isReadOnlyText;
            }
            set
            {
                SetProperty(ref _isReadOnlyText, value);
            }
        }

        //自己定义文本显示，文本框不显示
        private bool _customText = false;
        public bool CustomText
        {
            get
            {
                return _customText;
            }
            set
            {
                SetProperty(ref _customText, value);
            }
        }

        protected override void ClearText()
        {
            ShowText = false;

        }

        private void ExecuteSelectItemCommand(object param)
        {
            SelectItem((bool)param, !IsSelected);
        }

        private void SelectItem(bool newselect, bool select)
        {
            if (newselect)
            {
                foreach (var designerItemViewModelBase in Root.SelectedItems.ToList())
                {
                    designerItemViewModelBase.RemoveFromSelection();
                }
            }

            IsSelected = select;
        }

        public override void AddToSelection(bool selected, bool clearother)
        {
            if (clearother == true)
            {
                foreach (SelectableDesignerItemViewModelBase item in Root.SelectedItems.ToList())
                {
                    if (item != this)
                    {
                        item.RemoveFromSelection();
                    }
                }
            }

            if (selected == true)
            {
                Root.SelectionService.AddToSelection(this);
            }
            else
            {
                Root.SelectionService.RemoveFromSelection(this);
            }
        }


        public virtual void PreviewExecuteEdit()
        {

        }

        public virtual void ExitPreviewExecuteEdit()
        {
        }

        protected virtual void ExecuteEditCommand(object param)
        {
            if (IsReadOnly == true) return;

            ShowText = true;
        }

        protected virtual void ExecuteExitEditCommand(object param)
        {
            IsSelected = false;
        }
    }
}
