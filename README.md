# DragonUtilities

## Overview
The DragonUtilities Package is a versatile and powerful library designed to serve as the foundational framework for various personal and professional projects. It provides essential features and utilities for streamlined development, including logging, file service management, HTTP client services, caching, and web hosting.

This package is ideal for developers who wish to showcase their ability to create robust foundational utilities while also offering a reusable and centralised package for their projects.

---

## Purpose
The purpose of this package is to centralise commonly used functionalities in a single, easy-to-use library. By abstracting repetitive and foundational tasks, this utility ensures:

- **Improved Code Reusability**: Reduces boilerplate code across projects.
- **Standardised Practices**: Encourages consistency in logging, configuration, and service usage.
- **Enhanced Maintainability**: Simplifies updates and enhancements by consolidating core features into a single package.

This package is both a demonstration of development expertise and a tool that others may benefit from.

---

## Features

### **1. Logging Service**
A robust logging system to capture and manage application logs. The logging service includes:
- Support for logging messages at various levels: Debug, Info, Warning, Error, and Critical.
- Asynchronous logging for improved performance.
- Log file management, including size-based rotation and retention policies.
- Exception logging with detailed stack trace information.

**Example:**
```csharp
Logger.LogInfo("Application started successfully.");
Logger.LogError("An unexpected error occurred.");
```

### **2. HTTP Client Service**
A simplified interface for making HTTP requests, enabling seamless communication with external APIs.
- Supports GET, POST, PUT, and DELETE methods.
- Task-based asynchronous operations.
- Flexible request and response handling.

**Example:**
```csharp
var response = await HttpClientService.GetAsync("https://api.example.com/data");
Logger.LogDebug(response);
```

### **3. File Service Management**
A powerful service for managing file operations, replacing traditional configuration management.
- Read, write, append, and delete file operations.
- Supports serialisation and deserialisation of objects to/from files.
- Line-by-line file reading.

**Example:**
```csharp
// Writing to a file
FileService.WriteToFile("settings.json", appSettings);

// Reading from a file
var settings = FileService.ReadFromFile<AppSettings>("settings.json");

// Checking if a file exists
if (FileService.FileExists("settings.json"))
{
    Logger.LogInfo("Settings file exists.");
}
```

### **4. Web Hosting Service**
An abstraction for managing web hosting operations, including:
- Starting, stopping, and restarting web servers.
- Easy integration with hosted applications.

**Example:**
```csharp
WebHost.Start();
Logger.LogInfo("Web server started successfully.");
```

### **5. Caching Service**
A flexible caching mechanism for efficient data storage and retrieval.
- Supports setting and retrieving cache items synchronously and asynchronously.
- Includes dependency management and cache invalidation.
- Provides cache statistics.

**Example:**
```csharp
// Synchronous cache usage
CacheService.Set("key", data, TimeSpan.FromMinutes(5));
var cachedData = CacheService.Get<DataType>("key");

// Asynchronous cache usage
await CacheService.SetAsync("key", data, TimeSpan.FromMinutes(5));
var asyncData = await CacheService.GetAsync<DataType>("key");
```

### **6. Dependency Injection Support**
A flexible caching mechanism for efficient data storage and retrieval.
- Auto-wiring of dependencies using [Inject] attributes.
- Simplifies service management and reduces manual wiring efforts.

**Example:**
```csharp
[Inject]
public ILogger Logger { get; set; }

[Inject]
public IFileService FileService { get; set; }
    
[Inject]
public IHTTPClientService HttpClientService { get; set; }
    
[Inject]
public IWebHost WebHost { get; set; }
    
[Inject]
public ICacheService CacheService { get; set; }
```

---

## Installation

1. Add the NuGet package to your project:
   ```bash
   dotnet add package DragonUtilities
   ```
---

## Usage
To create a class that utilises the core utility features, you can simply inherit from `BaseController`:

```csharp
public class MyController : BaseController
{
    public void Execute()
    {
        Logger.LogInfo("Executing MyController logic.");

        var data = HttpClientService.GetAsync("https://api.example.com/data").Result;
        Logger.LogDebug($"Response: {data}");

        if (FileService.FileExists("settings.json"))
        {
            var settings = FileService.ReadFromFile<AppSettings>("settings.json");
            Logger.LogInfo("Settings loaded successfully.");
        }
    }
}
```

```csharp
public class MyController : BaseController
{
    public void Execute()
    {
        // Define the file path of the content you would like to use
        string filePath = "ExampleTypeApplication.json";

        // Create a ExampleTypeApplicaiton instance
        var testTypeInstance = new ExampleTypeApplicaiton()
        {
            Id = 1,
            Name = "Sample Data",
            CreatedAt = DateTime.UtcNow
        };

        // Write the TestType instance to a file
        Logger.LogInfo("Writing TestType instance to file...");
        FileService.WriteToFile(filePath, testTypeInstance);

        // Read the content back from the file
        Logger.LogInfo("Reading TestType instance from file...");
        var readInstance = FileService.ReadFromFile<ExampleTypeApplicaiton>(filePath);

        // Log the output
        if (readInstance != null)
        {
            Logger.LogInfo($"Read Content: ID={readInstance.Id}, Name={readInstance.Name}, CreatedAt={readInstance.CreatedAt}");
        }
        else
        {
            Logger.LogError("Failed to read content from file.");
        }
    }
}
```

## Usage without BaseController

If you would like to use the dependency without any use of the BaseController object you can directly hook it up within your targeted object with use of the ServiceFactory

```csharp
//Calculator object creation and Registration with DragonUtils
var calculator = new Calculator();
DragonUtilities.Instance.ServiceFactory(calculator);
calculator.Add(2, 4);

/// <summary>
/// Calculator Object
/// Simple type creation for testing purposes
/// </summary>
public class Calculator
{
    /// <summary>
    /// ILogger component marked with Injection Attribute for creation 
    /// </summary>
    [Inject]
    public ILogger Logger { get; set; }

    public int Add(int a, int b)
    {
        int result = a + b;
        Logger.LogInfo($"Adding {a} and {b} gives {result}");
        return result;
    }

    public int Subtract(int a, int b)
    {
        int result = a - b;
        Logger.LogInfo($"Subtracting {b} from {a} gives {result}");
        return result;
    }

    public int Multiply(int a, int b)
    {
        int result = a * b;
        Logger.LogInfo($"Multiplying {a} by {b} gives {result}");
        return result;
    }

    public int Divide(int a, int b)
    {
        if (b == 0)
        {
            Logger.LogError("Division by zero is not allowed.");
            throw new DivideByZeroException("Cannot divide by zero.");
        }

        int result = a / b;
        Logger.LogInfo($"Dividing {a} by {b} gives {result}");
        return result;
    }
}
```

---

## Configs

Some of the services have setting configs that will be located within your output build directory which you can use for adjusting the services.

### DragonUtils

```json
{
  "DebugMode": false
}
```

### Logging

```json
{
  "MinimumLogLevel": 1,
  "Filters": [],
  "LogToFile": false,
  "LogToConsole": true,
  "LogFileName": "Logs.log",
  "LogRetentionDays": 7,
  "MaxLogFileSize": 5242880,
  "IncludeTimestamp": true
}
```

### WebHost

```json
{
  "WebRootPath": "wwwroot",
  "Prefixes": [
    "http://localhost:8080/"
  ],
  "DefaultFile": "index.html",
  "MaxConnections": 100,
  "EnableLogging": true
}
```

---

## Update and Version History

### Version 1.0.0
- Initial release with logging, HTTP client, file service, caching, and web hosting services.
- Added dependency injection support.
- Added Main Config Utils for Debug handling
- Added Logging Config for Logging Service
- Added WebHost Config for HTTP Service

### Version 1.0.1
- Second release with minor fixes and refactoring.
- Chnage DragonUtilities type refactored to DragonUtilityManager
- Added API Documentation

---

## Author
**Name**: Dragonoid  
**GitHub**: https://github.com/DraconicKnights

---

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Disclaimer
This package is designed for personal projects and general use. It is provided "as-is" without warranties or guarantees of any kind.
