using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryInterfaces
{
	public interface IUnitOfWork
	{
		public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Serializable);
		public void Commit();
		public void Rollback();
	}
}
