using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tools.Json;

public class DefaultJsonSerializer
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new LowerCaseNamingStrategy()
        },
        Formatting = Formatting.None
    };
    
    public static string Serialize<T>(IEnumerable<T> sequence) => JsonConvert.SerializeObject(sequence, Settings);
    public static string Serialize<T>(T @object) => JsonConvert.SerializeObject(@object, Settings);
}