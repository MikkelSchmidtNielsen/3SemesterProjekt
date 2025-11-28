using Application.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
	public class UnitOfWork : IUnitOfWork
	{
		protected readonly SqlServerDbContext _dbContext;
		protected IDbContextTransaction? _transaction;

        public UnitOfWork(SqlServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Serializable)
		{
			if (_dbContext.Database.CurrentTransaction != null)
			{
				return;
			}

			_transaction = _dbContext.Database.BeginTransaction(isolationLevel);
		}

		public void Commit()
		{
			if (_transaction == null)
			{
				throw new Exception("You must call 'BeginTransaction' before Commit is called");
			}

			_transaction.Commit();
			_transaction.Dispose();
		}

		public void Rollback()
		{
			if (_transaction == null)
			{
				throw new Exception("You must call 'BeginTransaction' before Rollback is called");
			}

			_transaction.Rollback();
			_transaction.Dispose();
		}
	}
}
