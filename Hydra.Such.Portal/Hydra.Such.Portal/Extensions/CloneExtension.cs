using Newtonsoft.Json;

namespace Hydra.Such.Portal.Extensions
{
    public static class CloneExtension
    {
            public static T Clone<T>(this T source)
            {
                var serialized = JsonConvert.SerializeObject(source);
                return JsonConvert.DeserializeObject<T>(serialized);
            }
        }
}