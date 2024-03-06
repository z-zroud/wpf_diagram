using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDiagram.Interface
{
    public interface IAttachTo
    {
        bool BeAttachTo
        {
            get; set;
        }


        bool DisableAttachTo
        {
            get; set;
        }
    }
}
