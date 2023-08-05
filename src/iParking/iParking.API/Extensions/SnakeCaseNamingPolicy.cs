using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace iParking.API.Extensions
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        private readonly SnakeCaseNamingStrategy _newtonsoftSnakeCaseNamingStrategy;

        public SnakeCaseNamingPolicy(bool processDictionaryKeys, bool overrideSpecifiedNames)
        {
            _newtonsoftSnakeCaseNamingStrategy = new SnakeCaseNamingStrategy(processDictionaryKeys, overrideSpecifiedNames);
        }

        public SnakeCaseNamingPolicy()
        {
            _newtonsoftSnakeCaseNamingStrategy = new SnakeCaseNamingStrategy();
        }

        public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();

        public override string ConvertName(string name)
        {
            return _newtonsoftSnakeCaseNamingStrategy.GetPropertyName(name, false);
        }
    }
}
