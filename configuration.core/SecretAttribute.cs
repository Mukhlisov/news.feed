namespace configuration.core;

[AttributeUsage(AttributeTargets.Property)]
public class SecretAttribute : Attribute
{
    public string? Name { get; set; }
    
    /// <summary>
    /// По умолчанию Override = false. Это означает, что значения из ENV не будут переопределять явно заданные значения.
    /// <br/>
    /// Является некоторым решением для значимых типов, которые по умолчанию имеют какое-то значение.
    /// Тогда Override = true, постарается перезаписать его значением из ENV.
    /// </summary>
    public bool Override { get; set; } = false;
}