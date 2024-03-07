using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDiagram.Interface
{
    public interface ISelectable
    {
        bool IsSelected
        {
            get; set;
        }

        bool IsHitTestVisible
        {
            get;
        }

        bool IsEditing
        {
            get; set;
        }

        string Text
        {
            get; set;
        }

        //新建完处于编辑状态
        bool InitIsEditing
        {
            get; set;
        }

        bool ShowText
        {
            get; set;
        }

        void AddToSelection(bool selected, bool clearother);

        event PropertyChangedEventHandler PropertyChanged;
    }
}
