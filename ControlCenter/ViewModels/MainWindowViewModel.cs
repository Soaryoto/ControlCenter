// Copyright (C) 2026 Soaryoto
//
// This file is part of StreamingController.
//
// Licensed under the GNU General Public License v3.0 or later.
// See LICENSE for the full license text.
//
// SPDX-License-Identifier: GPL-3.0-or-later

using Soaryoto.Interfaces.Services;
using Soaryoto.ViewModels.Controls;
using Soaryoto.Infrastructure;

namespace Soaryoto.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private ILoggingService _loggingService;
    private ManagerControllableService _managerControllableService;
    private IProcessManagerService _processManagerService;
    public LogViewerViewModel LogsData { get; }
    public ServiceSwitcherViewModel ServiceSwitcherData { get; }
    public MainWindowViewModel(
        ILoggingService loggingService,
        ILoggingServiceReader loggingServiceReader,
        ManagerControllableService managerControllableService,
        IProcessManagerService processManagerService
        )
    {
        _loggingService = loggingService;
        _processManagerService = processManagerService;
        LogsData = new LogViewerViewModel(loggingServiceReader);
        _managerControllableService = managerControllableService;
        ServiceSwitcherData = new ServiceSwitcherViewModel(managerControllableService);
    }
}
