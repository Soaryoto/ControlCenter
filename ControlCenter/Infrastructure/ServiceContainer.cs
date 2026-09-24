// Copyright (C) 2026 Soaryoto
//
// This file is part of StreamingController.
//
// Licensed under the GNU General Public License v3.0 or later.
// See LICENSE for the full license text.
//
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Collections.Generic;
using System.Linq;

namespace Soaryoto.Infrastructure;

sealed class ServiceContainer
{
    private bool _isLocked = false;
    private readonly ServiceStore _serviceStore = new();
    public void RegisterSingleton<TClass>() where TClass : class
    {
        if (_isLocked)
            throw new InvalidOperationException($"Ошибка в регистрации сервиса, контейнер был закрыт");

        _serviceStore.RegisterSingleton<TClass>();
    }

    public void RegisterSingleton<TInterface, TClass>() where TClass : class where TInterface : class
    {
        if (_isLocked)
            throw new InvalidOperationException($"Ошибка в регистрации сервиса, контейнер был закрыт");

        _serviceStore.RegisterSingleton<TInterface, TClass>();
    }

    public void RegisterAlias<TInterface, TRegisteredClass>() where TInterface : class where TRegisteredClass : class
    {
        if (_isLocked)
            throw new InvalidOperationException($"Ошибка в регистрации сервиса, контейнер был закрыт");
        
        _serviceStore.RegisterAlias<TInterface, TRegisteredClass>();
    }

    public TInterface GetService<TInterface>() where TInterface : class => _serviceStore.GetService<TInterface>();

    public IEnumerable<TInterface> GetServices<TInterface>() where TInterface : class => _serviceStore.GetServices<TInterface>();

    public void Build()
    {
        _isLocked = true;
    }
}

sealed class ServiceStore
{
    private readonly Dictionary<Type, object> _services = new();
    private readonly Dictionary<Type, List<Type>> _aliases = new();

    private bool CheckContainService(Type type)
    {
        return _services.ContainsKey(type);
    }
    private object CreateInstance(Type implementationType)
    {
        var constructor = implementationType.GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length).First();
        var parameters = constructor.GetParameters();

        var args = parameters
            .Select(p => ResolveDependency(p.ParameterType))
            .ToArray();

        return constructor.Invoke(args);
    }

    private object ResolveDependency(Type type)
    {
        if(_aliases.TryGetValue(type, out var implementationTypes))
        {
            if(implementationTypes.Count > 0)
            {
                var implementationType = implementationTypes.First();
                if(_services.TryGetValue(implementationType, out var service))
                {
                    return service;
                }
            }
        }

        throw new InvalidOperationException(
            $"[SC] Не удалось создать сервис: зависимость {type.Name} не зарегистрирована. " +
            $"Зарегистрируй её ДО сервиса, который от неё зависит.");
    }

    public void RegisterSingleton<TClass>()
        where TClass : class
    {
        _services[typeof(TClass)] = CreateInstance(typeof(TClass));
    }
    public void RegisterSingleton<TInterface, TClass>()
        where TClass : class
        where TInterface : class
    {
        RegisterSingleton<TClass>();
        RegisterAlias<TInterface, TClass>();
    }
    public void RegisterAlias<TInterface, TRegisteredClass>()
        where TInterface : class
        where TRegisteredClass : class
    {
        if(!CheckContainService(typeof(TRegisteredClass)))
            throw new InvalidOperationException($"[SC] Сервис {typeof(TRegisteredClass).Name} не зарегистрирован.");

        if(_aliases.TryGetValue(typeof(TInterface), out var implementationTypes))
        {
            implementationTypes.Add(typeof(TRegisteredClass));
        }
        else
        {
            _aliases[typeof(TInterface)] = new List<Type> { typeof(TRegisteredClass) };
        }
    }

    public TInterface GetService<TInterface>() where TInterface : class
    {
        if(_aliases.TryGetValue(typeof(TInterface), out var implementationTypes))
        {
            if(implementationTypes.Count > 0)
            {
                var implementationType = implementationTypes.First();
                if(_services.TryGetValue(implementationType, out var service))
                {
                    return (TInterface)service;
                }
            }
        }
        throw new InvalidOperationException($"[SC] Сервис {typeof(TInterface).Name} не зарегистрирован.");
    }

    public IEnumerable<TInterface> GetServices<TInterface>() where TInterface : class 
    {
        if(_aliases.TryGetValue(typeof(TInterface), out var implementationTypes))
        {
            List<TInterface> servicesList = new List<TInterface>();
            foreach(var implementationType in implementationTypes)
            {
                if(_services.TryGetValue(implementationType, out var service))
                {
                    servicesList.Add((TInterface)service);
                }
            }
            return servicesList;
        }
        throw new InvalidOperationException($"[SC] Сервис {typeof(TInterface).Name} не зарегистрирован.");
    }
}