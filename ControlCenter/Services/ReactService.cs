// Copyright (C) 2026 Soaryoto
//
// This file is part of StreamingController.
//
// Licensed under the GNU General Public License v3.0 or later.
// See LICENSE for the full license text.
//
// SPDX-License-Identifier: GPL-3.0-or-later

using Soaryoto.Interfaces.Services;
using Soaryoto.Models;

namespace Soaryoto.Services;

sealed class ReactService : BaseControllableService, IReactService
{
    private readonly string _reactPath;
    private readonly ManagedProcess _reactProcess;
    private readonly IProcessManagerService _processManagerService;

    public ReactService(IProjectEnvironmentService projectEnvironmentService, IProcessManagerService processManagerService)
    {
        Name = ServicesName.React;
        _reactPath = projectEnvironmentService.ReactPath;
        _reactProcess = new ManagedProcess()
        {
            Name = "ReactAvatar",
            ProcessTypeName = ProcessTypeName.ReactAvatar,
            Executable = "cmd.exe",
            Arguments = "/C npm run preview",
            WorkingDirectory = _reactPath
        };

        _processManagerService = processManagerService;
        _processManagerService.AddProcessInfo(_reactProcess);
    }

    public void Toggle()
    {
        if (State == ServicesState.Stopped)
        {
            _processManagerService.StartProcess(_reactProcess.ProcessTypeName);
            State = ServicesState.Started;
        }
        else
        {
            _processManagerService.StopProcess(_reactProcess.ProcessTypeName);
            State = ServicesState.Stopped;
        }
    }


}