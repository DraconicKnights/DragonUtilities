using System.Text.Json;
using DragonUtilities.Core.Json;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Handler;

public class ConfigurationBuilder<T> where T : class, IConfig, new()
{
    private readonly Dictionary<string, object> _configDictionary;

    public ConfigurationBuilder()
    {
        _configDictionary = new Dictionary<string, object>();
    }

    public ConfigurationBuilder<T> AddSettings(string key, object value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Value cannot be null.");
        }
        
        if (_configDictionary.ContainsKey(key))
        {
            _configDictionary[key] = value;
        }
        else
        {
            _configDictionary.Add(key, value);
        }

        return this;
    }

    public ConfigurationBuilder<T> RemoveSettings(string key)
    {
        if (_configDictionary.ContainsKey(key))
        {
            _configDictionary.Remove(key);
        }

        return this;
    }

    public T? Build()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new LogDestinationConverter() }
        };

        var json = JsonSerializer.Serialize(_configDictionary);
        return JsonSerializer.Deserialize<T>(json, jsonOptions);
    }

    public void SaveToFile(string filePath)
    {
        var json = JsonSerializer.Serialize(_configDictionary, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public ConfigurationBuilder<T> LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Configuration file not found.");

        var json = File.ReadAllText(filePath);
        var configDic = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

        if (configDic == null)
            throw new Exception("Failed to parse configuration file.");

        foreach (var kvp in configDic)
        {
            AddSettings(kvp.Key, kvp.Value);
        }

        return this;
    }

    public void DisplayConfig()
    {
        foreach (var kvp in _configDictionary)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value} {Environment.NewLine}");
        }
    }
}