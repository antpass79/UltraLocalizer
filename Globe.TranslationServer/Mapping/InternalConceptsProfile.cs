using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;

namespace Globe.TranslationServer.Mapping
{
    public class InternalConceptsProfile : Profile
    {
        public InternalConceptsProfile()
        {
            CreateMap<InternalConceptsTable, InternalConceptsDTO>();
            CreateMap<InternalConceptsDTO, InternalConceptsTable>();
        }
    }
}