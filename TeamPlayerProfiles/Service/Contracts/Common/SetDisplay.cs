namespace Service.Contracts.Common
{
    public static class SetDisplay
    {
        public sealed class Request()
        {
            public Guid Id { get; set; }

            public bool Displayed { get; set; }
        }
    }
}
