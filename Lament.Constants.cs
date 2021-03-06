using System;

namespace Lament
{
    public static class Constants
    {
        public const string ENVIRONMENT_VARIABLE_PREFIX = "LAMENT_";
        public const string ENVIRONMENT_VARNAME = ENVIRONMENT_VARIABLE_PREFIX + "ENVIRONMENT";
        public const string DEFAULT_RUNTIME_ENVIRONMENT = "Development";

        public const string CONFIGURATION_FILENAME = "lament.appsettings.json";

        public const string DEFAULT_GUILD_MESSAGE_PREFIX = "lm ";

        public static Guid SessionId = Guid.NewGuid();
    }
}