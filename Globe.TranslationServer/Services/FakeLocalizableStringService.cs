using Globe.TranslationServer.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class FakeLocalizableStringService : IAsyncLocalizableStringService
    {
        private List<LocalizableString> _strings = new List<LocalizableString>();

        public FakeLocalizableStringService()
        {
            _strings = new List<LocalizableString>
            {
                new LocalizableString
                {
                    Id = Guid.NewGuid(),
                    Key = "STRING_0",
                    Language = "en",
                    Value = "STRING 0",
                    SavedOn = DateTime.Now
                },
                new LocalizableString
                {
                    Id = Guid.NewGuid(),
                    Key = "STRING_1",
                    Language = "en",
                    Value = "STRING 1",
                    SavedOn = DateTime.Now
                },
                new LocalizableString
                {
                    Id = Guid.NewGuid(),
                    Key = "STRING_2",
                    Language = "en",
                    Value = "STRING 2",
                    SavedOn = DateTime.Now
                }
            };
        }

        async public Task<IEnumerable<LocalizableString>> GetAllAsync()
        {
            return await Task.FromResult(_strings);
        }

        async public Task SaveAsync(IEnumerable<LocalizableString> strings)
        {
            foreach (var @string in strings)
            {
                var stringToUpdate = _strings.Find(item => item.Key == @string.Key);
                if (stringToUpdate != null)
                {
                    stringToUpdate.Value = @string.Value;
                    stringToUpdate.SavedOn = DateTime.Now;
                }
                else
                {
                    @string.SavedOn = DateTime.Now;
                    _strings.Add(@string);
                }
            }

            await Task.CompletedTask;
        }
    }
}
