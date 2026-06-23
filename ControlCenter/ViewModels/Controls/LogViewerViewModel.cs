using System;
using System.IO;
using ControlCenter.Models;

namespace ControlCenter.ViewModels.Controls;

public partial class LogViewerViewModel : ViewModelBase
{
    public LogModel Logs { get; }
    public LogViewerViewModel(LogModel logs)
    {
        Logs = logs;
    }
}
