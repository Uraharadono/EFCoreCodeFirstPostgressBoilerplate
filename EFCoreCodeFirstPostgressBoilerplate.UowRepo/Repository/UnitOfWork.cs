using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using EFCoreCodeFirstPostgressBoilerplate.UowRepo.Events;
using EFCoreCodeFirstPostgressBoilerplate.UowRepo.Exceptions;
using EFCoreCodeFirstPostgressBoilerplate.UowRepo.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCoreCodeFirstPostgressBoilerplate.UowRepo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;

        public Microsoft.EntityFrameworkCore.DbContext Context { get; private set; }

        /// <summary>
        ///     Gets the name of the connection string.
        /// </summary>
        public string ConnectionStringName => "";
            // Context.Database.Connection.ConnectionString;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <param name="ctx">The database context.</param>
        public UnitOfWork(Microsoft.EntityFrameworkCore.DbContext ctx)
        {
            ctx.EnsureNotNull();
            Context = ctx;
        }

        /// <summary>
        ///     Occurs when an entity is updated.
        /// </summary>
        public event EventHandler<EntityEventArgs> EntityUpdated;

        /// <summary>
        ///     Determines whether the unit of work is in a transaction.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the unit is part of a transaction.
        /// </returns>
        public bool IsInTransaction()
        {
            return _transaction != null;
        }

        /// <summary>
        ///     Executes the action in a transaction.
        /// </summary>
        /// <param name="action">The action that needs to be wrapped in a transaction.</param>
        public void ExecuteInTransaction(Action action)
        {
            bool isAlreadyInTransaction = IsInTransaction();

            try
            {
                if (!isAlreadyInTransaction) BeginTransaction();
                action();
                if (!isAlreadyInTransaction) CommitTransaction();
            }
            catch (Exception)
            {
                if (!isAlreadyInTransaction)
                    RollbackTransaction();
                throw;
            }
        }

        /// <summary>
        ///     Determines whether this instance has changes.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if unit of work has changes for the database.
        /// </returns>
        public bool HasChanges()
        {
            return GetChanges().Any();
        }

        public void Commit()
        {
            Commit(false);
        }

        /// <summary>
        ///     Saves the changes.
        /// </summary>
        /// <exception cref="AppException">SaveChanges failed:\r\n{0} \r\n {1}</exception>
        public void Commit(bool skipCacheUpdate)
        {
            try
            {
                // collect entries to fire events after save changes
                var modifiedEntries = skipCacheUpdate
                    ? null
                    : GetChanges().ToArray();

                Context.SaveChanges();

                // fire entries updated
                if (modifiedEntries != null && modifiedEntries.Any() && EntityUpdated != null)
                {
                    var modifiedTypes = modifiedEntries.Select(e => e.GetType()).Distinct().ToArray();
                    EntityUpdated(this, new EntityEventArgs(modifiedTypes));
                }
            }
            catch (Exception e)
            {
                TryRejectChanges();
                throw new AppException("SaveChanges failed:\r\n{0} \r\n {1}", e.ToString(), "");
            }
            // TODO: Figure this effing shite out
            // https://github.com/dotnet/efcore/issues/4434
            // https://entityframeworkcore.com/knowledge-base/46430619/-net-core-2---ef-core-error-handling-save-changes
            // 
            //catch (DbEntityValidationException ex)
            //{
            //    if (IsInTransaction())
            //        RollbackTransaction();

            //    var msg = new StringBuilder();
            //    foreach (var eve in ex.EntityValidationErrors)
            //    {
            //        var entry = eve?.Entry;
            //        if (entry == null) continue;

            //        msg.AppendFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            entry.Entity.GetType().Name, entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            msg.AppendFormat("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }

            //    TryRejectChanges();

            //    throw new AppException("SaveChanges failed:\r\n{0} \r\n {1}", ex.ToString(), msg.ToString());
            //}
        }

        /// <summary>
        ///     Deletes all tables from the database.
        /// </summary>
        public void ClearDb(params string[] excluded)
        {
#if (STAGING || RELEASE)
                throw new Exception("Clearing a production database is not allowed!");
#endif

            // TODO: Figure this out: https://stackoverflow.com/questions/35631903/raw-sql-query-without-dbset-entity-framework-core
            //var db = Context.Database;
            //var tableNames = db
            //    .SqlQuery<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME <> '__MigrationHistory'")
            //    .Where(name => excluded == null || !excluded.Contains(name))
            //    .ToList();

            //foreach (var tableName in tableNames)
            //    db.ExecuteSqlCommand($"ALTER TABLE {tableName} NOCHECK CONSTRAINT ALL");
            //foreach (var tableName in tableNames)
            //    db.ExecuteSqlCommand($"DELETE FROM {tableName}; DBCC CHECKIDENT ({tableName}, RESEED, 0)");
            //foreach (var tableName in tableNames)
            //    db.ExecuteSqlCommand($"ALTER TABLE {tableName} CHECK CONSTRAINT ALL");

            Context.SaveChanges();
        }

        public void TruncTables(params string[] tables)
        {
#if (STAGING || RELEASE)
                throw new Exception("Truncating production tables is not allowed!");
#endif

            // TODO: Figure this out: https://stackoverflow.com/questions/35631903/raw-sql-query-without-dbset-entity-framework-core
            //var db = Context.Database;
            //var tableNames = db
            //    .SqlQuery<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME <> '__MigrationHistory'")
            //    .Where(tables.Contains)
            //    .ToList();

            //foreach (var tableName in tableNames)
            //    db.ExecuteSqlCommand($"ALTER TABLE {tableName} NOCHECK CONSTRAINT ALL");
            //foreach (var tableName in tableNames)
            //    db.ExecuteSqlCommand($"DELETE FROM {tableName}; DBCC CHECKIDENT ({tableName}, RESEED, 0)");
            //foreach (var tableName in tableNames)
            //    db.ExecuteSqlCommand($"ALTER TABLE {tableName} CHECK CONSTRAINT ALL");

            Context.SaveChanges();
        }

        public void Dispose()
        {
            if (IsInTransaction())
                RollbackTransaction();

            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }

        /// <summary>
        ///     Gets the repository of <typeparamref name="T" /> of the database.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>
        ///     The repository
        /// </returns>
        public IRepository<T> Repo<T>() where T : class, IEntity
        {
            return new Repository<T>(this);
        }

        /// <summary>
        ///     Gets the queryable of <typeparamref name="T" /> of the database.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>
        ///     A query
        /// </returns>
        public IQueryable<T> GetQueryable<T>() where T : class, IEntity
        {
            return Context.Set<T>().AsQueryable();
        }

        /// <summary>
        ///     Executes raw SQL on the database.
        /// </summary>
        /// <param name="sql">The query.</param>
        /// <param name="parameters">The parameters.</param>
        public void ExecuteSql(string sql, params object[] parameters)
        {
            Context.Database.ExecuteSqlCommand(sql, parameters);
        }

        private void BeginTransaction()
        {
            if (IsInTransaction())
                throw new AppException("Already in transaction.");

            _transaction = Context.Database.BeginTransaction();
        }

        private void CommitTransaction()
        {
            if (!IsInTransaction())
                throw new AppException("Not in transaction.");

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        private void RollbackTransaction()
        {
            if (!IsInTransaction())
                throw new AppException("Not in transaction");

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        private void TryRejectChanges()
        {
            try
            {
                foreach (var entry in Context.ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                            
                        case EntityState.Deleted:
                            entry.State = EntityState.Unchanged;
                            break;
                            
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("TryRejectChanges silently failed: {0}", ex.Message);
            }
        }

        private IEnumerable<object> GetChanges()
        {
            return Context.ChangeTracker.Entries()
                .Where(
                    t =>
                        t.State == EntityState.Added || t.State == EntityState.Modified ||
                        t.State == EntityState.Deleted)
                .Select(e => e.Entity);
        }

        public IQueryable<T> Query<T>() where T : class, IEntity
        {
            return Context.Set<T>();
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> filter) where T : class, IEntity
        {
            return Context.Set<T>().Where(filter);
        }
    }
}