using AutoMapper;
using Globe.Shared.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;

namespace Globe.TranslationServer.Mapping
{
    public class InternalNamespaceProfile : Profile
    {
        public InternalNamespaceProfile()
        {
            CreateMap<InternalConceptsTable, InternalNamespace>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.InternalNamespace)); ;
            CreateMap<InternalNamespace, InternalConceptsTable>()
                .ForMember(dest => dest.InternalNamespace, opt => opt.MapFrom(src => src.Description)); ;
        }
    }
}