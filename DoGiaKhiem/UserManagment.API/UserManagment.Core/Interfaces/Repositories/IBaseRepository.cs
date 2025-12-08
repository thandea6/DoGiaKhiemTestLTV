using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagment.Core.Interfaces.Repositories
{
    public interface IBaseRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(Guid entityId);
        T Add(T entity);
        T Update(T entity);
        void Delete(Guid entityId);
    }
}
