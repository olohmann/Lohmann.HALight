using Lohmann.HALight.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lohmann.HALight.Formatters.Internal
{
    /// <summary>
    /// Helper class which provides <see cref="JsonSerializerSettings"/>.
    /// </summary>
    internal static class HalJsonSerializerSettingsProvider
    {
        public const string HalMediaType = "application/hal+json";

        private const int DefaultMaxDepth = 32;

        /// <summary>
        /// Creates default <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <returns>Default <see cref="JsonSerializerSettings"/>.</returns>
        public static JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                // Limit the object graph we'll consume to a fixed depth. This prevents stackoverflow exceptions
                // from deserialization errors that might occur from deeply nested objects.
                MaxDepth = DefaultMaxDepth,

                // Do not change this setting
                // Setting this to None prevents Json.NET from loading malicious, unsafe, or security-sensitive types
                TypeNameHandling = TypeNameHandling.None
            };

            return settings;
        }

        public static JsonSerializerSettings AppendHalConverters(JsonSerializerSettings settings)
        {
            settings.Converters.Add(new RelationsConverter());
            settings.Converters.Add(new ResourceConverter());
            return settings;
        }
    }
}