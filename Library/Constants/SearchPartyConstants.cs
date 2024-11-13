using Library.Utils;

namespace Library.Constants
{
    public static class SearchPartyConstants
    {
        public static readonly int MinMmr;

        public static readonly int MaxMmr;

        public static readonly TimeSpan MessageExpirationTime;

        static SearchPartyConstants()
        {
            MinMmr = EnvironmentUtils.TryGetEnvVariable("MIN_MMR", out var minMmr) && int.TryParse(minMmr, out var minMmrInt) ? minMmrInt : 0;

            MinMmr = EnvironmentUtils.TryGetEnvVariable("MAX_MMR", out var maxMmr) && int.TryParse(maxMmr, out var maxMmrInt) ? maxMmrInt : 20000;

            MessageExpirationTime = EnvironmentUtils.TryGetEnvVariable("MESSAGE_EXPIRATION_TIME", out var time) && int.TryParse(time, out var timeInt) ? TimeSpan.FromMinutes(timeInt) : TimeSpan.FromMinutes(30);
        }
    }
}
