using System.Collections.ObjectModel;

namespace Soaryoto.Models;

public class LogModel
{
    public ObservableCollection<LogEntry> Entries { get; } = new ();
}