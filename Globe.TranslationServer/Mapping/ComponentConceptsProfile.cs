using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;

namespace Globe.TranslationServer.Mapping
{
    public class ComponentConceptsProfile : Profile
    {
        public ComponentConceptsProfile()
        {
            CreateMap<ComponentConceptsTable, ComponentConceptsDTO>();
            CreateMap<ComponentConceptsDTO, ComponentConceptsTable>();
        }
    }
}
