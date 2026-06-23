using System.Collections.ObjectModel;

namespace ControlCenter.Models;

public class LogModel
{
    public ObservableCollection<LogEntry> Entries { get; } = new ();
}