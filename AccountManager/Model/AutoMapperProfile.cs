using AutoMapper;

namespace AccountManager.Model
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AccountDTO, Account>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.DomainName, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.DomainName) ? Constants._defaultDomainName : src.DomainName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.Encrypt()));

            CreateMap<Account, AccountDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.Decrypt()));

            CreateMap<Account, AccountDetailedDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.Decrypt()));
        }
    }
}
