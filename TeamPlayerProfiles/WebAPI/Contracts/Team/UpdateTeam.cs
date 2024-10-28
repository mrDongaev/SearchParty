﻿using System.ComponentModel.DataAnnotations;

namespace WebAPI.Contracts.Team
{
    public static class UpdateTeam
    {
        public sealed class Request
        {
            [Required]
            public Guid? Id { get; set; }

            [MaxLength(30)]
            public string? Name { get; set; }

            [MaxLength(150)]
            public string? Description { get; set; }
        }
    }
}
