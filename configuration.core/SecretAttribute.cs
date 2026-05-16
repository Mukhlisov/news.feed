namespace configuration.core;

[AttributeUsage(AttributeTargets.Property)]
public class SecretAttribute : Attribute
{
    public string? Name { get; set; }
    
    /// <summary>
    /// По умолчанию Override = true. Это означает, что значения из ENV будут переопределять явно заданные значения.
    /// </summary>
    public bool Override { get; set; } = true;
}