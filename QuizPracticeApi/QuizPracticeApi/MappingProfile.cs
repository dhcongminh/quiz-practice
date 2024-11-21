using AutoMapper;
using QuizPracticeApi.Models;
using QuizPracticeApi.Services;

namespace QuizPracticeApi {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserDetail.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserDetail.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserDetail.Email))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.UserDetail.Gender))
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.UserDetail.Dob))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserDetail.Avatar));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.UserDetail, opt => opt.MapFrom(src => new UserDetail {
                    FirstName = src.FirstName,
                    LastName = src.LastName,
                    Email = src.Email,
                    Gender = src.Gender,
                    Dob = src.Dob,
                    Avatar = src.Avatar
                }));
        }
    }
}
