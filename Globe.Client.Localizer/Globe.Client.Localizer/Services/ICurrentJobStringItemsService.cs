using Globe.Client.Localizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface ICurrentJobStringItemsService
    {
        Task<IEnumerable<StringViewItem>> GetStringViewItemsAsync(StringViewItemSearch search);
    }
}
