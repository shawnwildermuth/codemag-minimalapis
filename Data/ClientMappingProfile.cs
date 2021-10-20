using AutoMapper;

namespace MinimalApis.Data;

public class ClientMappingProfile : Profile
{
  public ClientMappingProfile()
  {
    CreateMap<Client, ClientModel>()
      .ForMember(c => c.Address1, o => o.MapFrom(m => m.Address.Address1))
      .ForMember(c => c.Address2, o => o.MapFrom(m => m.Address.Address2))
      .ForMember(c => c.Address3, o => o.MapFrom(m => m.Address.Address3))
      .ForMember(c => c.CityTown, o => o.MapFrom(m => m.Address.CityTown))
      .ForMember(c => c.StateProvince, o => o.MapFrom(m => m.Address.StateProvince))
      .ForMember(c => c.PostalCode, o => o.MapFrom(m => m.Address.PostalCode))
      .ForMember(c => c.Country, o => o.MapFrom(m => m.Address.Country))
      .ReverseMap();

  }
}