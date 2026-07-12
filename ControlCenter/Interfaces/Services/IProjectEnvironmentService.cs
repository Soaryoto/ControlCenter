namespace Soaryoto.Interfaces.Services;
public interface IProjectEnvironmentService
{
    string ProjectRootPath { get; }
    string LogsPath { get; }
    string ReactPath { get; }
}