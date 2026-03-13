using System;
using System.Collections.Generic;
using System.Text;

namespace Servr.Domain.Interface
{
  public interface ILogger
  {
    void Log(string level, string message);
  }
}
