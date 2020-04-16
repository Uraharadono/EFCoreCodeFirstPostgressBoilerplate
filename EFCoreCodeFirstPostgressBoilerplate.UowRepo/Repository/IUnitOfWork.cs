using System;
using System.Linq;
using System.Linq.Expressions;

namespace EFCoreCodeFirstPostgressBoilerplate.UowRepo.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        Microsoft.EntityFrameworkCore.DbContext Context { get; }
        string ConnectionStringName { get; }

        bool HasChanges();
        bool IsInTransaction();
        
        void ExecuteInTransaction(Action action);
        void ExecuteSql(string sql, params object[] parameters);

        void ClearDb(params string[] excluded);
        void TruncTables(params string[] tables);
        void Commit();
        void Commit(bool skipCacheUpdate);

        IRepository<T> Repo<T>() where T : class, IEntity;
        IQueryable<T> Query<T>() where T : class, IEntity;
        IQueryable<T> Query<T>(Expression<Func<T, bool>> filter) where T : class, IEntity;
        IQueryable<T> GetQueryable<T>() where T : class, IEntity;
    }
}