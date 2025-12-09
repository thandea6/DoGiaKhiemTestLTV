using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagment.Core.Interfaces.Repositories;
using UserManagment.Core.Interfaces.Services;

namespace UserManagment.Core.Services
{
    public class BaseService<T> : IBaseService<T>
    {
        private IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public T Add(T entity)
        {
            try
            {
                var res = _repository.Add(entity);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting entity", ex);
            }
        }

        public bool Delete(Guid entityId)
        {
            try
            {
                var idExist = _repository.GetById(entityId);
                if (idExist == null)
                {
                    return false;
                }

                _repository.Delete(entityId);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting entity", ex);
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                var data = _repository.GetAll();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving data", ex);
            }
        }

        public T GetById(Guid entityId)
        {
            try
            {
                var data = _repository.GetById(entityId);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving entity with ID: {entityId}", ex);
            }
        }

        public T Update(T entity)
        {
            try
            {
                var data = _repository.Update(entity);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating entity", ex);
            }
        }
    }
}
