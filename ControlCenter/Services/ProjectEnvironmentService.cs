// Copyright (C) 2026 Soaryoto
//
// This file is part of StreamingController.
//
// Licensed under the GNU General Public License v3.0 or later.
// See LICENSE for the full license text.
//
// SPDX-License-Identifier: GPL-3.0-or-later

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
    public string ReactPath { get; private set; } = string.Empty;
    public ProjectEnvironmentService()
    {
        ProjectRootPath = FindPath(_rootSettingsDirName);
        LogsPath = CheckPathExists(Path.Combine(ProjectRootPath, _rootSettingsDirName, "logs"));
        ReactPath = Path.Combine(ProjectRootPath, "client");
    }

    private string CheckPathExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
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