using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WpfDiagram.ViewModels;

namespace WpfDiagram.Interface
{
    public interface ILockObjectViewModel
    {
        List<LockObject> LockObject { get; set; }
        void SetValue(LockObject obj);
        event PropertyChangedEventHandler PropertyChanged;
    }
}
