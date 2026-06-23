using System.Diagnostics;
using System;
using CommunityToolkit.Mvvm.Input;
using ControlCenter.Models;
using ControlCenter.ViewModels.Controls;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ControlCenter.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private int logCounter;
    private readonly LogModel _serverLog = new();
    public LogViewerViewModel ServerLog { get; }

    public MainWindowViewModel()
    {
        logCounter = 4;
        ServerLog = new LogViewerViewModel(_serverLog);
    }

    [RelayCommand]
    private void AddTestLog()
    {
        LogCounter++;
        ServerLog.Logs.Entries.Add(new LogEntry
        {
            Time = DateTime.Now,
            Level = LogLevel.Info,
            Message = "Тестовое сообщение" + LogCounter.ToString()
        });
    }

}
