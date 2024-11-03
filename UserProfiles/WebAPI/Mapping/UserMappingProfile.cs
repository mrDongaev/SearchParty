﻿using AutoMapper;
using Common.Models;
using Library.Models.Enums;
using Library.Models.QueryConditions;
using Service.Contracts.User;
using WebAPI.Models.User;

namespace WebAPI.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDto, GetUser.Response>();

            CreateMap<CreateUser.Request, CreateUserDto>();

            CreateMap<UpdateUser.Request, UpdateUserDto>()
                .ForMember(d => d.Id, m => m.Ignore());

            CreateMap<GetConditionalUser.Request, ConditionalUserQuery>();
        }
    }
}
