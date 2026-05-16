namespace configuration.core.Exceptions;

public class EnvVarNotFoundException : Exception
{
    public EnvVarNotFoundException() { }
    public EnvVarNotFoundException(string message) : base(message) { }
}