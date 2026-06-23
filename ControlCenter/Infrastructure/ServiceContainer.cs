using System;
using System.Collections.Generic;
using System.Linq;

namespace Soaryoto.Infrastructure;

sealed class ServiceContainer
{
    private bool _isLocked = false;
    private readonly Dictionary<Type, object> _services = new();

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
        if (_services.TryGetValue(type, out var existing))
            return existing;

        throw new InvalidOperationException(
            $"[SC] Не удалось создать сервис: зависимость {type.Name} не зарегистрирована. " +
            $"Зарегистрируй её ДО сервиса, который от неё зависит.");
    }

    public void RegisterSingleton<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, TInterface
    {
        if (_isLocked)
            throw new InvalidOperationException($"Ошибка в регистрации сервиса, контейнер был закрыт");
            
        object instance = CreateInstance(typeof(TImplementation));
        _services[typeof(TInterface)] = instance;
    }

    public void RegisterAlias<TNewInterface, TRegisteredInterface>()
        where TNewInterface : class
        where TRegisteredInterface : class
    {
        if (_isLocked)
            throw new InvalidOperationException($"Ошибка в регистрации сервиса, контейнер был закрыт");
            
        if (!_services.TryGetValue(typeof(TRegisteredInterface), out var instance))
            throw new InvalidOperationException($"Ошибка! В контейнере нед подходящего объекта");

        _services[typeof(TNewInterface)] = instance;
    }

    public T GetService<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }
        throw new InvalidOperationException($"[SC] Сервис {typeof(T).Name} не зарегистрирован.");
    }

    public void Build()
    {
        _isLocked = true;
    }
}