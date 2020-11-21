using AutoMapper;
using TechLibrary.Contracts.Requests;
using TechLibrary.Domain;

namespace TechLibrary.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<UpdateBookRequest, Book>().ForMember(x => x.ShortDescr, opt => opt.MapFrom(src => src.Descr));
        }
    }
}
