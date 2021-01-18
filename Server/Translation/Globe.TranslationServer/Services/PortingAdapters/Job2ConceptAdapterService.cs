using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class Job2ConceptAdapterService : IAsyncJob2ConceptService
    {
        private readonly LocalizationContext _context;
        private readonly UltraDBJob2Concept _ultraDBJob2Concept;

        public Job2ConceptAdapterService(LocalizationContext context, UltraDBJob2Concept ultraDBJob2Concept)
        {
            _context = context;
            _ultraDBJob2Concept = ultraDBJob2Concept;
        }

        public Task DeleteAsync(LocJob2Concept job2Concept)
        {
            _ultraDBJob2Concept.DeleteJob2Concept(job2Concept.IdjobList);
            throw new NotImplementedException();

//            return Task.CompletedTask;
        }

        async public Task<IEnumerable<LocJob2Concept>> GetAllAsync()
        {
            return await Task.FromResult(_context.LocJob2Concepts);
        }

        public Task<LocJob2Concept> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(LocJob2Concept job2Concept)
        {
            _ultraDBJob2Concept.AppendConcept2JobList(job2Concept.IdjobList, job2Concept.Idconcept2Context);
            throw new NotImplementedException();
        }

        public Task UpdateAsync(LocJob2Concept job2Concept)
        {
            throw new NotImplementedException();
        }
    }
}
