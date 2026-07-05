using System;
using System.Collections.ObjectModel;
using System.IO;
using Soaryoto.Interfaces.Services;
using Soaryoto.Models;

namespace Soaryoto.Services;

public sealed class LoggingService : ILoggingService, ILoggingServiceReader
{
    private static readonly string LogFilePath = "logs/log{0}.log";
    private static readonly string LogFormat = "[{0}] [{1}] {2}\n";

    private ObservableCollection<LogEntry> _entries { get; } = new ();

    public LoggingService() { }

    public void WriteLine(string message, LogLevel logLevel = LogLevel.Info)
    {
        LogEntry newEntry = new LogEntry
        {
            Time = DateTime.Now,
            Level = logLevel,
            Message = message
        };
        File.AppendAllText(string.Format(LogFilePath, DateTime.Now.ToString("yyyy-MM-dd")), string.Format(LogFormat, newEntry.Time, newEntry.Level, newEntry.Message));
        _entries.Add(newEntry);
    }

    public ReadOnlyObservableCollection<LogEntry> GetLogs()
    {
        return new ReadOnlyObservableCollection<LogEntry>(_entries);
    }
}