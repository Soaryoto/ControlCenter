using System.Collections.ObjectModel;
using Soaryoto.Interfaces.Services;
using Soaryoto.Models;

namespace Soaryoto.ViewModels.Controls;

public partial class LogViewerViewModel : ViewModelBase
{
    public ReadOnlyObservableCollection<LogEntry> Logs { get; }
    public LogViewerViewModel(ILoggingServiceReader loggingServiceReader)
    {
        Logs = loggingServiceReader.GetLogs();
    }
}
