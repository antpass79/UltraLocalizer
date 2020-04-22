﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncWriteService<T>
    {
        Task InsertAsync(T job2Concept);
        Task UpdateAsync(T job2Concept);
        Task DeleteAsync(T job2Concept);
    }
}
