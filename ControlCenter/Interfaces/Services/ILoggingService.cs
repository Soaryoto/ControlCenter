using Soaryoto.Models;

namespace Soaryoto.Interfaces.Services;

public interface ILoggingService
{
    void WriteLine(string message, LogLevel logLevel = LogLevel.Info);
}