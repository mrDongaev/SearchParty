using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class CommonUtils
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

        public static string TryGetEnvVariable(string key)
        {
            var variable = Environment.GetEnvironmentVariable(key);
            return variable ?? string.Empty;
        }
    }
}
