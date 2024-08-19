﻿using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories.Models;
using Service.Contracts.Player;
using Service.Services.Interfaces.Common;
using static Common.Models.ConditionalQuery;

namespace Service.Services.Implementations.PlayerServices
{
    public class PlayerBoardService(IMapper mapper, IPlayerRepository playerRepo) : IBoardService<PlayerDto, PlayerConditions>
    {
        public async Task<PlayerDto?> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken)
        {
            var player = new Player() { Id = id, Displayed = displayed };
            var updatedPlayer = await playerRepo.Update(player, cancellationToken);
            return updatedPlayer == null ? null : mapper.Map<PlayerDto>(updatedPlayer);
        }

        public async Task<ICollection<PlayerDto>> GetFiltered(PlayerConditions query, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetConditionalPlayerRange(query, cancellationToken);
            return mapper.Map<ICollection<PlayerDto>>(players);
        }

        public async Task<PaginatedResult<PlayerDto>> GetPaginated(PlayerConditions query, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetPaginatedPlayerRange(query, page, pageSize, cancellationToken);
            return mapper.Map<PaginatedResult<PlayerDto>>(players);
        }
    }
}