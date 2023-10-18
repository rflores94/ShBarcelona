using AutoMapper;
using ShBarcelona.DAL.Entities;
using ShBarcelona.Services.Area;

namespace ShBarcelona.Services
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<AreaEntity, AreaDto>().ReverseMap();
        }
    }
}
