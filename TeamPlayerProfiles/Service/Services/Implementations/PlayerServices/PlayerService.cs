using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using FluentResults;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Contracts.Player;
using Service.Services.Interfaces.PlayerInterfaces;

namespace Service.Services.Implementations.PlayerServices
{
    public class PlayerService(IMapper mapper, IPlayerRepository playerRepo, IUserHttpContext userContext) : IPlayerService
    {
        public async Task<Result<PlayerDto>> Create(CreatePlayerDto dto, CancellationToken cancellationToken = default)
        {
            Player newPlayer = mapper.Map<Player>(dto);
            Player createdPlayer = await playerRepo.Add(newPlayer, cancellationToken);
            return Result.Ok(mapper.Map<PlayerDto>(createdPlayer));
        }

        public async Task<Result<bool>> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var userId = await playerRepo.GetProfileUserId(id, cancellationToken);
            if (userId != userContext.UserId)
            {
                return Result.Fail<bool>(new UnauthorizedError()).WithValue(false);
            }

            var result = await playerRepo.Delete(id, cancellationToken);

            if (result)
            {
                return Result.Ok(true);
            }

            return Result.Fail<bool>(new EntityNotFoundError("Player with the given ID has not been found")).WithValue(false);
        }

        public async Task<Result<PlayerDto?>> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var player = await playerRepo.Get(id, cancellationToken);

            if (player == null)
            {
                return Result.Fail<PlayerDto?>(new EntityNotFoundError("Player with the given ID has not been found"));
            }

            if (!player.Displayed.HasValue || (!player.Displayed.Value && player.UserId != userContext.UserId))
            {
                return Result.Fail<PlayerDto?>(new UnauthorizedError()).WithValue(null);
            }

            return Result.Ok(mapper.Map<PlayerDto?>(player));
        }

        public async Task<Result<ICollection<PlayerDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetAll(cancellationToken);

            players = players.Where(p => p.UserId == userContext.UserId || (p.Displayed.HasValue && p.Displayed.Value)).ToList();

            if (players.Count == 0)
            {
                return Result.Fail<ICollection<PlayerDto>>(new EntitiesNotFoundError("No players have been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<PlayerDto>>(players));
        }

        public async Task<Result<ICollection<PlayerDto>>> GetProfilesByUserId(Guid userId, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetProfilesByUserId(userId, cancellationToken);

            if (userId != userContext.UserId)
            {
                players = players.Where(p => p.Displayed.HasValue && p.Displayed.Value).ToList();
            }

            if (players.Count == 0)
            {
                return Result.Fail<ICollection<PlayerDto>>(new EntityRangeNotFoundError("No players of user with the given ID have been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<PlayerDto>>(players));
        }

        public async Task<Result<Guid?>> GetProfileUserId(Guid profileId, CancellationToken cancellationToken = default)
        {
            Guid? userId = await playerRepo.GetProfileUserId(profileId, cancellationToken);

            if (userId.HasValue)
            {
                return Result.Ok(userId);
            }

            return Result.Fail<Guid?>(new EntityNotFoundError("No users corresponding to the given player ID have been found")).WithValue(null);
        }

        public async Task<Result<ICollection<PlayerDto>>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetRange(ids, cancellationToken);

            players = players.Where(p => p.UserId == userContext.UserId || (p.Displayed.HasValue && p.Displayed.Value)).ToList();

            if (players.Count == 0)
            {
                return Result.Fail<ICollection<PlayerDto>>(new EntityRangeNotFoundError("No players with the given IDs have been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<PlayerDto>>(players));
        }

        public async Task<Result<PlayerDto?>> Update(UpdatePlayerDto dto, CancellationToken cancellationToken = default)
        {
            var player = mapper.Map<Player>(dto);

            Guid? userId = await playerRepo.GetProfileUserId(player.Id, cancellationToken);

            if (!userId.HasValue || userId.Value != userContext.UserId)
            {
                return Result.Fail<PlayerDto?>(new UnauthorizedError()).WithValue(null);
            }

            var updatedPlayer = await playerRepo.Update(player, dto.HeroIds, cancellationToken);

            if (updatedPlayer == null)
            {
                return Result.Fail<PlayerDto?>(new EntityNotFoundError("Player with the given ID has not been found")).WithValue(null);
            }

            return Result.Ok(mapper.Map<PlayerDto?>(updatedPlayer));
        }
    }
}
