using System.Diagnostics;
using Servr.Domain.Interface;

namespace Servr.Infrastructure.Logger;

public class DebugLogger : ILogger
{
  public void Log(string level, string message)
  {
    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{level}] - {message}");
  }
}
