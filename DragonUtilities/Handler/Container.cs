using DragonUtilities.Attributes;

namespace DragonUtilities.Handler;

public class Container
{
    private readonly Dictionary<Type, Type> _registrations = new();
    private readonly Dictionary<Type, object> _instances = new();
    private readonly Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();
    private readonly object _lock = new(); // For thread-safety

    public Container()
    {
        AutoRegisterServices();
    }
    
    public TInterface Resolve<TInterface>()
    {
        return (TInterface)Resolve(typeof(TInterface));
    }

    private object Resolve(Type type)
    {
        // Check if it's a primitive, string, or value type
        if (type.IsPrimitive || type == typeof(string) || type.IsValueType)
        {
            if (_factories.TryGetValue(type, out var factory))
            {
                return factory();
            }

            // Provide default value for primitives and structs
            return Activator.CreateInstance(type)!;
        }

        // Check if an existing instance is available
        lock (_lock)
        {
            if (_instances.TryGetValue(type, out var instance))
            {
                return instance;
            }
        }

        // Check for a registered factory
        if (_factories.TryGetValue(type, out var factoryMethod))
        {
            var instance = factoryMethod();
            lock (_lock) { _instances[type] = instance; }
            return instance;
        }

        // Find the implementation type
        if (!_registrations.TryGetValue(type, out var implementationType))
        {
            throw new InvalidOperationException($"Type {type.FullName} is not registered.");
        }

        // Resolve constructor dependencies
        var constructor = implementationType.GetConstructors().FirstOrDefault();
        if (constructor == null)
        {
            throw new InvalidOperationException($"No suitable constructor found for {implementationType.FullName}.");
        }

        var parameters = constructor.GetParameters()
            .Select(param => Resolve(param.ParameterType))
            .ToArray();

        // Create the instance
        var newInstance = Activator.CreateInstance(implementationType, parameters);
        InjectProperties(newInstance);

        // Store the instance
        lock (_lock)
        {
            _instances[type] = newInstance;
        }

        return newInstance;
    }

    public void InjectProperties(object instance)
    {
        var properties = instance.GetType().GetProperties()
            .Where(prop => prop.IsDefined(typeof(InjectAttribute), true));

        foreach (var property in properties)
        {
            DragonUtilityManager.Instance.DebugLog($"Injecting property {property.Name} of type {property.PropertyType.Name}");
            var value = Resolve(property.PropertyType);
            if (value == null)
            {
                DragonUtilityManager.Instance.DebugLog($"Failed to resolve {property.PropertyType.Name}");
            }
            property.SetValue(instance, value);
        }
    }

    public void Register<TInterface, TImplementation>() where TImplementation : TInterface
    {
        lock (_lock)
        {
            _registrations[typeof(TInterface)] = typeof(TImplementation);
        }    
    }

    public void Register(Type interfaceType, Type implementationType)
    {
        lock (_lock)
        {
            _registrations[interfaceType] = implementationType;
        }    
    }
    
    public void Register<T>(Func<object> factory)
    {
        lock (_lock)
        {
            _factories[typeof(T)] = factory;
        }    
    }

    public void RegisterInstance<T>(T instance)
    {
        lock (_lock)
        {
            _instances[typeof(T)] = instance;
        }
    }

    private void AutoRegisterServices()
    {
        var allTypes = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => assembly.FullName != null && !assembly.FullName.StartsWith("System") && !assembly.FullName.StartsWith("Microsoft"))
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract &&
                           (type.IsDefined(typeof(RegisterInjectAttribute), true) ||
                            type.GetProperties().Any(prop => prop.IsDefined(typeof(InjectAttribute), true))));

        foreach (var type in allTypes)
        {
            var interfaces = type.GetInterfaces();
            if (interfaces.Any())
            {
                Register(interfaces.First(), type); // Register the first interface implemented
            }
            else
            {
                Register(type, type); // Register the type itself if no interface is implemented
            }
        }
    }

    public void InjectDependenciesIntoObject(object instance)
    {
        DragonUtilityManager.Instance.DebugLog($"Injecting dependencies into {instance.GetType().Name}");
        InjectProperties(instance);
    }
}