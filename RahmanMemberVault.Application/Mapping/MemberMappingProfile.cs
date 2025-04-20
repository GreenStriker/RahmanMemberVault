using System;
using AutoMapper;
using RahmanMemberVault.Application.DTOs;
using RahmanMemberVault.Core.Entities;

namespace RahmanMemberVault.Application.Mapping
{
    // AutoMapper profile for mapping between Member entity and DTOs.
    public class MemberMappingProfile : Profile
    {
        public MemberMappingProfile()
        {
            // Entity -> DTO
            CreateMap<Member, MemberDto>();

            // Create DTO -> Entity
            CreateMap<CreateMemberDto, Member>()
                .ForMember(dest => dest.DateJoined, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));

            // Update DTO -> Entity
            CreateMap<UpdateMemberDto, Member>();
        }
    }
}
