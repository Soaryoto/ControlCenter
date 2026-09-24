// Copyright (C) 2026 Soaryoto
//
// This file is part of StreamingController.
//
// Licensed under the GNU General Public License v3.0 or later.
// See LICENSE for the full license text.
//
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;
using System.Linq;
using Soaryoto.Interfaces.Services;
using Soaryoto.Models;

namespace Soaryoto.Infrastructure;

public sealed class ManagerControllableService
{
    private readonly IEnumerable<IControllableService> _services;
    public IReadOnlyList<IControllableService> Services
    {
        get
        {
            return _services.ToList();
        }
    }

    public ManagerControllableService(IEnumerable<IControllableService> services)
    {
        _services = services;
    }

    public void StartService(ServicesName serviceName)
    {
        foreach (var service in _services)
        {
            if (service.Name == serviceName)
            {
                service.Toggle();
                break;
            }
        }
    }
}