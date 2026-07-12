using System;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Threading;
using Soaryoto.Interfaces.Services;
using Soaryoto.Models;

namespace Soaryoto.Services;

public sealed class LoggingService : ILoggingService, ILoggingServiceReader
{
    private readonly object _fileLock = new();
    private static readonly string _logPath = "logs";
    private static readonly string _logFilePath = "{0}/log{1}.log";
    private static readonly string _logFormat = "[{0}] [{1}] {2}\n";

    private ObservableCollection<LogEntry> _entries { get; } = new ();
    private readonly ReadOnlyObservableCollection<LogEntry> _readOnlyEntries;

    public LoggingService()
    {
        _readOnlyEntries = new ReadOnlyObservableCollection<LogEntry>(_entries);
    }

    public void WriteLine(string message, LogLevel logLevel = LogLevel.Info)
    {
        LogEntry newEntry = new LogEntry
        {
            Time = DateTime.Now,
            Level = logLevel,
            Message = message
        };
        lock (_fileLock) WriteInFile(newEntry);
        
        Dispatcher.UIThread.Post(() =>
        {
            _entries.Add(newEntry);
        });
    }

    private void WriteInFile(LogEntry entry)
    {
        if (!Directory.Exists(_logPath))
        {
            Directory.CreateDirectory(_logPath);
        }
        File.AppendAllText(string.Format(_logFilePath, _logPath, DateTime.Now.ToString("yyyy-MM-dd")), string.Format(_logFormat, entry.Time, entry.Level, entry.Message));
    }

    public ReadOnlyObservableCollection<LogEntry> GetLogs()
    {
        return _readOnlyEntries;
    }
}