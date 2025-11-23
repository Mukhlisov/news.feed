using Newtonsoft.Json.Serialization;

namespace Tools.Json;

public class LowerCaseNamingStrategy : NamingStrategy
{
    protected override string ResolvePropertyName(string name) => name.ToLower();
}