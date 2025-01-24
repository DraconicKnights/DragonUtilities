using System.Text.Json;
using DragonUtilities.Handler;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Utils;

public static class ConfigLoader
{
    public static T Load<T>(string folder, string fileName, Func<T> defaultConfigProvider) where T : class, IConfig, new()
    {
        string configFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder);
        string configFilePath = Path.Combine(configFolder, fileName);

        if (!Directory.Exists(configFolder)) Directory.CreateDirectory(configFolder);

        if (!File.Exists(configFilePath))
        {
            var defaultConfig = defaultConfigProvider.Invoke();
            var json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configFilePath, json);
        }

        var configBuilder = new ConfigurationBuilder<T>().LoadFromFile(configFilePath);
        var config = configBuilder.Build();

        if (config == null)
        {
            throw new Exception($"Failed to build configuration for {typeof(T).Name} from file: {configFilePath}");
        }

        return config;
    }
}