using System;
using System.Collections.ObjectModel;
using Soaryoto.Interfaces.Services;
using Soaryoto.Models;

namespace Soaryoto.Services;

sealed class LoggingService : ILoggingService, ILoggingServiceReader
{
    private ObservableCollection<LogEntry> _entries { get; } = new ();

    public LoggingService() { }

    public void WriteLine(string message, LogLevel logLevel = LogLevel.Info)
    {
        _entries.Add(new LogEntry
        {
            Time = DateTime.Now,
            Level = logLevel,
            Message = message
        });
    }

    public ReadOnlyObservableCollection<LogEntry> GetLogs()
    {
        return new ReadOnlyObservableCollection<LogEntry>(_entries);
    }
}