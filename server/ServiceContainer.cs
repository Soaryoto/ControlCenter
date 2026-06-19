using Soaryoto.Interfaces;

namespace Soaryoto
{
    public class ServiceContainer
    {
        private readonly Dictionary<Type, object> _services = new();

        private object CreateInstance(Type implementationType)
        {
            var constructor = implementationType.GetConstructors().First();
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
            object instance = CreateInstance(typeof(TImplementation));
            _services[typeof(TInterface)] = instance;
        }

        public T GetService<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"[SC] Сервис {typeof(T).Name} не зарегистрирован.");
        }

        public void StartAll()
        {
            foreach (var service in _services.Values.OfType<ILaunchedService>())
            {
                service.StartService();
            }
        }

        public void StopAll()
        {
            foreach (var service in _services.Values.OfType<ILaunchedService>())
            {
                service.StopService();
            }
        }
    }
}