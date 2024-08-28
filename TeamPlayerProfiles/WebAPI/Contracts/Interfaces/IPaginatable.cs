namespace WebAPI.Contracts.Interfaces
{
    public interface IPaginatable
    {
        uint? Page {  get; set; }

        uint? PageSize { get; set; }
    }
}
