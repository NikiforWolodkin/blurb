using AutoMapper;
using courseproject_api.Dtos;
using courseproject_api.Models;

namespace courseproject_api.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDto>();
        }
    }
}
