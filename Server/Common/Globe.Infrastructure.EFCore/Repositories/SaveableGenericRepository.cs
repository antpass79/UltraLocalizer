using Globe.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Globe.Infrastructure.EFCore.Repositories
{
    public class SaveableGenericRepository<TContext, TEntity> : GenericRepository<TContext, TEntity>, ISaveable
        where TContext : DbContext
        where TEntity : class
    {
        readonly private bool _saveOnChange;

        public SaveableGenericRepository(TContext context, bool saveOnChange = false)
            : base(context)
        {
            _saveOnChange = saveOnChange;
        }

        public override void Insert(TEntity entity)
        {
            base.Insert(entity);

            if (_saveOnChange)
                this.Save();
        }

        public override void Update(TEntity entity)
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

    public class AsyncSaveableGenericRepository<TContext, TEntity> : AsyncGenericRepository<TContext, TEntity>, IAsyncSaveable
        where TContext : DbContext
        where TEntity : class
    {
        readonly private bool _saveOnChange;

        public AsyncSaveableGenericRepository(TContext context, bool saveOnChange = false)
            : base(context)
        {
            _saveOnChange = saveOnChange;
        }

        async public override Task InsertAsync(TEntity entity)
        {
            await base.InsertAsync(entity);

            if (_saveOnChange)
                await this.SaveAsync();
        }

        async public override Task UpdateAsync(TEntity entity)
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
