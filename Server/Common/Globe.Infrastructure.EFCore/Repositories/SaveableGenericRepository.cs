using Globe.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Globe.Infrastructure.EFCore.Repositories
{
    public class SaveableGenericRepository<T> : GenericRepository<T>, ISaveable
        where T : class
    {
        readonly private bool _saveOnChange;

        public SaveableGenericRepository(DbContext context, bool saveOnChange = false)
            : base(context)
        {
            _saveOnChange = saveOnChange;
        }

        public override void Insert(T entity)
        {
            base.Insert(entity);

            if (_saveOnChange)
                this.Save();
        }

        public override void Update(T entity)
        {
            base.Update(entity);

            if (_saveOnChange)
                this.Save();
        }

        public void Save()
        {
            this._context.SaveChanges();
        }
    }

    public class AsyncSaveableGenericRepository<T> : AsyncGenericRepository<T>, IAsyncSaveable
        where T : class
    {
        readonly private bool _saveOnChange;

        public AsyncSaveableGenericRepository(DbContext context, bool saveOnChange = false)
            : base(context)
        {
            _saveOnChange = saveOnChange;
        }

        async public override Task InsertAsync(T entity)
        {
            await base.InsertAsync(entity);

            if (_saveOnChange)
                await this.SaveAsync();
        }

        async public override Task UpdateAsync(T entity)
        {
            await base.UpdateAsync(entity);

            if (_saveOnChange)
                await this.SaveAsync();
        }

        async public Task SaveAsync()
        {
            await this._context.SaveChangesAsync();
        }
    }
}
