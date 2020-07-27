using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class ContextService : IAsyncContextService
    {
        private readonly IMapper _mapper;
        private readonly LocalizationContext _context;

        public ContextService(IMapper mapper, LocalizationContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        async public Task<IEnumerable<ContextDTO>> GetAllAsync()
        {
            var result = _context.LocContexts.ToList();
            return await Task.FromResult(_mapper.Map<IEnumerable<ContextDTO>>(result));
        }

        public Task<ContextDTO> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
