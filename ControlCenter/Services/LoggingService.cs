// Copyright (C) 2026 Soaryoto
//
// This file is part of StreamingController.
//
// Licensed under the GNU General Public License v3.0 or later.
// See LICENSE for the full license text.
//
// SPDX-License-Identifier: GPL-3.0-or-later

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
    private readonly string _logPath;
    private readonly string _logFilePath;
    private readonly string _logFormat;

    private ObservableCollection<LogEntry> _entries { get; } = new ();
    private readonly ReadOnlyObservableCollection<LogEntry> _readOnlyEntries;

    public LoggingService(IProjectEnvironmentService projectEnvironmentService)
    {
        _logPath = projectEnvironmentService.LogsPath;
        _logFilePath = "{0}/log{1}.log";
        _logFormat = "[{0}] [{1}] {2}\n";
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
        File.AppendAllText(string.Format(_logFilePath, _logPath, DateTime.Now.ToString("yyyy-MM-dd")), string.Format(_logFormat, entry.Time, entry.Level, entry.Message));
    }

    public ReadOnlyObservableCollection<LogEntry> GetLogs()
    {
        return _readOnlyEntries;
    }
}