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