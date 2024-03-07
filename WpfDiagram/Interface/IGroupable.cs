using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDiagram.Interface
{
    public interface IGroupable
    {
        Guid Id { get; }
        Guid ParentId { get; set; }
        bool IsGroup { get; set; }
    }
}
