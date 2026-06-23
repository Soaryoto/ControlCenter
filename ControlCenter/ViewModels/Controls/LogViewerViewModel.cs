using System.Collections.ObjectModel;
using Soaryoto.Interfaces.Services;
using Soaryoto.Models;

namespace Soaryoto.ViewModels.Controls;

public partial class LogViewerViewModel : ViewModelBase
{
    private ILoggingServiceReader _loggingServiceReader;
    public ReadOnlyObservableCollection<LogEntry> Logs => _loggingServiceReader.GetLogs();
    public LogViewerViewModel(ILoggingServiceReader loggingServiceReader)
    {
        _loggingServiceReader = loggingServiceReader;
    }
}
