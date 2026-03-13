using System;
using System.Collections.Generic;
using System.Text;

namespace Servr.Domain.Interface
{
    public interface IMenuItem : IItem
    {
        string Category { get; }
    }
}
