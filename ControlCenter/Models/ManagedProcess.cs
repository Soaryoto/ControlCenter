using System.Diagnostics;

namespace Soaryoto.Models;

public enum ProcessTypeName
{
    ReactAvatar
}

public class ManagedProcess
{
    public ProcessTypeName ProcessTypeName { get; init; }
    public string Name { get; init; } = "";

    public string Executable { get; init; } = "";

    public string Arguments { get; init; } = "";

    public string WorkingDirectory { get; init; } = "";

    public Process? Process { get; set; }
}