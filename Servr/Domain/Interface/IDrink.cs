using System;
using System.Collections.Generic;
using System.Text;

namespace Servr.Domain.Interface
{
    public interface IDrink : IItem
    {
        int Volume { get; }
    }
}
