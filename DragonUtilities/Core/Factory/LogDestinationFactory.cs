using DragonUtilities.Interfaces;

namespace DragonUtilities.Core.Factory;

public static class LogDestinationFactory
{
    public static ILogDestination Create(string typeName)
    {
        // Attempt to get the type
        var type = Type.GetType(typeName);

        // If the type is not found, search all loaded assemblies
        if (type == null)
        {
            type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == typeName);
        }

        // Validate the type
        if (type == null || !typeof(ILogDestination).IsAssignableFrom(type))
        {
            throw new InvalidOperationException($"Invalid log destination type: {typeName}");
        }

        // Create an instance of the log destination
        return (ILogDestination)Activator.CreateInstance(type)!;
    }
}