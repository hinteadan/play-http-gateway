using H.Necessaire;
using Microsoft.Extensions.Configuration;

namespace H.Config
{
    internal static class ConfigXtnx
    {
        public static IConfigurationSection GetConfigSection(this IConfiguration config, params string[] path)
        {
            if (config is null)
                return null;

            if (path.IsEmpty())
                return null;

            IConfigurationSection result = null;
            foreach (string part in path)
            {
                result = result?.GetSection(part) ?? config?.GetSection(part);
                if (result is null || result.GetChildren().IsEmpty())
                    return null;
            }

            return result;
        }

        public static string GetConfigValue(this IConfigurationSection config, params string[] path)
        {
            return config.GetConfigSection(path)?.Value;
        }
        public static string[] GetConfigValues(this IConfigurationSection config, params string[] path)
        {
            return config.GetConfigSection(path)?.GetChildren()?.Select(x => x.Value).ToNonEmptyArray();
        }

    }
}
