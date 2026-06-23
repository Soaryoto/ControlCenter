using System.Collections.ObjectModel;
using Soaryoto.Models;

namespace Soaryoto.Interfaces.Services;

public interface ILoggingServiceReader
{
    ReadOnlyObservableCollection<LogEntry> GetLogs();
}