namespace Library.Utils
{
    public static class EnvironmentUtils
    {
        public static string GetEnvVariable(string key)
        {
            var variable = Environment.GetEnvironmentVariable(key);
            if (variable == null)
            {
                throw new ApplicationException($"Value of environment variable \"{key}\" not found");
            }
            return variable;
        }

        public static bool TryGetEnvVariable(string key, out string? envVariable)
        {
            envVariable = Environment.GetEnvironmentVariable(key);
            return envVariable != null;
        }
    }
}
