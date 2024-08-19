namespace WebAPI.Contracts.Interfaces
{
    public interface IPaginatable
    {
        int? Page {  get; set; }

        int? PageSize { get; set; }
    }
}
