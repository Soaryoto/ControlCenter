using Soaryoto.Models;

namespace Soaryoto.Interfaces.Services;

public interface IProcessManagerService
{
    void Build();
    void AddProcessInfo(ManagedProcess processStartInfo);
    void StartProcess(ProcessTypeName processTypeName);
    void StopProcess(ProcessTypeName processTypeName);
    void RestartProcess(ProcessTypeName processTypeName);
    void IsRunning(ProcessTypeName processTypeName);
    void StopAllProcess();
}