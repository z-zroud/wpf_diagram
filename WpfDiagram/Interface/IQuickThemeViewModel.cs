using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WpfDiagram.ViewModels;

namespace WpfDiagram.Interface
{
    public interface IQuickThemeViewModel
    {
        QuickTheme[] QuickThemes { get; }
        QuickTheme QuickTheme { get; set; }
        event PropertyChangedEventHandler PropertyChanged;

    }
}
