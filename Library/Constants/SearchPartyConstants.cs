using Library.Utils;

namespace Library.Constants
{
    public static class SearchPartyConstants
    {
        public static readonly int MinMmr;

        public static readonly int MaxMmr;

        public static readonly TimeSpan MessageExpirationTime;

        public static readonly int MaxCount;

        static SearchPartyConstants()
        {
            MinMmr = EnvironmentUtils.TryGetEnvVariable("MAX_COUNT", out var maxCount) && int.TryParse(maxCount, out var maxCountInt) ? maxCountInt : 0;

            MinMmr = EnvironmentUtils.TryGetEnvVariable("MIN_MMR", out var minMmr) && int.TryParse(minMmr, out var minMmrInt) ? minMmrInt : 0;

            MinMmr = EnvironmentUtils.TryGetEnvVariable("MAX_MMR", out var maxMmr) && int.TryParse(maxMmr, out var maxMmrInt) ? maxMmrInt : 20000;

            MessageExpirationTime = EnvironmentUtils.TryGetEnvVariable("MESSAGE_EXPIRATION_TIME", out var time) && int.TryParse(time, out var timeInt) ? TimeSpan.FromMinutes(timeInt) : TimeSpan.FromMinutes(30);
        }
    }
}
