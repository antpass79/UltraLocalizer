using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;

namespace Globe.TranslationServer.Mapping
{
    public class ComponentNamespaceProfile : Profile
    {
        public ComponentNamespaceProfile()
        {
            CreateMap<ComponentConceptsTable, ComponentNamespaceDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ComponentNamespace)); ;
            CreateMap<ComponentNamespaceDTO, ComponentConceptsTable>()
                .ForMember(dest => dest.ComponentNamespace, opt => opt.MapFrom(src => src.Description)); ;
        }
    }
}
