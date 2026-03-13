using System;
using System.Collections.Generic;
using System.Text;
using Servr.Domain.Interface;

namespace Servr.Infrastructure.Logger
{
  public class Logger : ILogger
  {
    public void Log(string level, string message)
    {
      string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] - {message}";
      // implement save to file
    }
  }
}
