using Soaryoto.Models;

namespace Soaryoto.ViewModels.Controls;

public partial class LogViewerViewModel : ViewModelBase
{
    public LogModel Logs { get; }
    public LogViewerViewModel(LogModel logs)
    {
        Logs = logs;
    }
}
