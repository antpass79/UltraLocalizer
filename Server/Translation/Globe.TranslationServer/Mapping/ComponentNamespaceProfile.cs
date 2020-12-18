using AutoMapper;
using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;

namespace Globe.TranslationServer.Mapping
{
    public class ComponentNamespaceProfile : Profile
    {
        public ComponentNamespaceProfile()
        {
            CreateMap<ComponentConceptsTable, ComponentNamespace>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ComponentNamespace)); ;
            CreateMap<ComponentNamespace, ComponentConceptsTable>()
                .ForMember(dest => dest.ComponentNamespace, opt => opt.MapFrom(src => src.Description)); ;
        }
    }
}
