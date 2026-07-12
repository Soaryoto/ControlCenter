using System.Collections.Generic;
using Soaryoto.Interfaces.Services;
using Soaryoto.Models;

namespace Soaryoto.Infrastructure;

public sealed class ManagerControllableService
{
    private readonly IEnumerable<IControllableService> _services;
    public IEnumerable<IControllableService> Services => _services;

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