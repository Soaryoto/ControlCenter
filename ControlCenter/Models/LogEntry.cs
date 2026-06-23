using System;

namespace Soaryoto.Models;

public enum LogLevel
{
    Info,
    Warning,
    Error,
    Debug
}

public class LogEntry
{
    public DateTime Time { get; init; }

    public LogLevel Level { get; init; }

    public string Message { get; init; } = "";
}