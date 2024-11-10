using Library.Services.Interfaces;

namespace Service.Services.Interfaces
{
    public interface IMessageService<TGetDto, TCreateDto> : IService<TGetDto, Guid>, IRangeGettable<TGetDto, Guid>
    {
    }
}
