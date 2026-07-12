using System;
using System.IO;
using System.Linq;
using Soaryoto.Interfaces.Services;

namespace Soaryoto.Services;

public sealed class ProjectEnvironmentService : IProjectEnvironmentService
{
    private readonly string _rootSettingsDirName = ".soaryoto";
    public string ProjectRootPath { get; private set; } = string.Empty;
    public string LogsPath { get; private set; } = string.Empty;
    public ProjectEnvironmentService()
    {
        ProjectRootPath = FindPath(_rootSettingsDirName);
        LogsPath = Path.Combine(ProjectRootPath, _rootSettingsDirName, "logs");
    }

    private string FindPath(string pathName)
    {
        DirectoryInfo? current = new(AppContext.BaseDirectory);

        while (current is not null)
        {
            if (current.EnumerateDirectories(pathName).Any())
                return current.FullName;

            current = current.Parent;
        }
        throw new DirectoryNotFoundException($"Не удалось найти корневую директорию проекта. Убедитесь, что в проекте присутствует папка {pathName}.");
    }
}