using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;

namespace Globe.TranslationServer.Mapping
{
    public class InternalNamespaceProfile : Profile
    {
        public InternalNamespaceProfile()
        {
            CreateMap<InternalConceptsTable, InternalNamespaceDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.InternalNamespace)); ;
            CreateMap<InternalNamespaceDTO, InternalConceptsTable>()
                .ForMember(dest => dest.InternalNamespace, opt => opt.MapFrom(src => src.Description)); ;
        }
    }
}