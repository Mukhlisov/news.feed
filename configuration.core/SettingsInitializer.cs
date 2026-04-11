using System.Reflection;

namespace configuration.core;

public static class SettingsInitializer
{
    private static readonly Dictionary<Type, Func<string, object>> DefaultTypeConverters = new()
    {
        {
            typeof(int),
            value => int.TryParse(value, out var result)
                ? result
                : throw new Exception($"Failed to convert '{value}' to int")
        },
        {
            typeof(long),
            value => long.TryParse(value, out var result)
                ? result
                : throw new Exception($"Failed to convert '{value}' to long")
        },
        {
            typeof(double),
            value => double.TryParse(value, out var result)
                ? result
                : throw new Exception($"Failed to convert '{value}' to double")
        },
        {
            typeof(bool),
            value => bool.TryParse(value, out var result)
                ? result
                : throw new Exception($"Failed to convert '{value}' to bool")
        },
        {
            typeof(Guid),
            value => Guid.TryParse(value, out var result)
                ? result
                : throw new Exception($"Failed to convert '{value}' to Guid")
        },
        {
            typeof(DateTime),
            value => DateTime.TryParse(value, out var result)
                ? result
                : throw new Exception($"Failed to convert '{value}' to DateTime")
        },
        {
            typeof(string), value => value
        }
    };

    public static object[] InitSettings()
    {
        var settingInstances = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .Where(t => t.GetCustomAttribute<SettingAttribute>() is not null)
            .Select(Activator.CreateInstance)
            .Where(x => x is not null)
            .ToArray();

        foreach (var instance in settingInstances)
        {
            var secretProps = instance!.GetType().GetProperties()
                .Select(prop => (prop, prop.GetCustomAttribute<SecretAttribute>()))
                .Where(x => x.Item2 is not null)
                .ToList();
            InitProps(secretProps, instance);
        }

        return settingInstances!;
    }

    private static void InitProps(List<(PropertyInfo, SecretAttribute?)> secretProps, object instance)
    {
        foreach (var (prop, attribute) in secretProps)
            if (attribute!.Override || prop.GetValue(instance) is null)
                prop.SetValue(instance, GetValueFromEnvironment(prop, attribute!.Name));
    }

    private static object GetValueFromEnvironment(PropertyInfo prop, string? name)
    {
        var envName = name ?? prop.Name;
        var value = Environment.GetEnvironmentVariable(envName)
                    ?? throw new Exception($"ENV variable: '{envName}' not found");

        return DefaultTypeConverters[prop.PropertyType].Invoke(value);
    }
}