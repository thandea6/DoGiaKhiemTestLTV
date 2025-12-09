using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagment.Core.Interfaces.Repositories;
using UserManagment.Core.Interfaces.Services;

namespace UserManagment.Core.Services
{
    /// <summary>
    /// Lớp service cơ sở thực hiện business logic cho các thao tác CRUD
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu Entity</typeparam>
    /// Created by: DGKhiem (09/12/2025)
    public class BaseService<T> : IBaseService<T>
    {
        /// <summary>
        /// Repository để truy cập dữ liệu từ database
        /// </summary>
        private IBaseRepository<T> _repository;

        /// <summary>
        /// Constructor khởi tạo BaseService với Repository tương ứng.
        /// </summary>
        /// <param name="repository">Đối tượng Repository được tiêm vào từ Dependency Injection</param>
        /// <exception cref="ArgumentNullException">Ném ngoại lệ nếu repository là null</exception>
        /// Created by: DGKhiem (09/12/2025)
        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Thêm một đối tượng mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm</param>
        /// <returns>Đối tượng vừa được thêm vào cơ sở dữ liệu</returns>
        /// <exception cref="Exception">Ném ngoại lệ nếu có lỗi khi thêm đối tượng</exception>
        /// Created by: DGKhiem (09/12/2025)
        public virtual T Add(T entity)
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

        /// <summary>
        /// Xóa một đối tượng dựa trên ID.
        /// </summary>
        /// <param name="entityId">ID của đối tượng cần xóa</param>
        /// <returns>True nếu xóa thành công, False nếu không tìm thấy đối tượng</returns>
        /// <exception cref="Exception">Ném ngoại lệ nếu có lỗi khi xóa đối tượng</exception>
        /// Created by: DGKhiem (09/12/2025)
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

        /// <summary>
        /// Lấy tất cả các đối tượng từ cơ sở dữ liệu.
        /// </summary>
        /// <returns>Danh sách các đối tượng</returns>
        /// <exception cref="Exception">Ném ngoại lệ nếu có lỗi khi lấy dữ liệu</exception>
        /// Created by: DGKhiem (09/12/2025)
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

        /// <summary>
        /// Lấy một đối tượng theo ID.
        /// </summary>
        /// <param name="entityId">ID của đối tượng cần tìm</param>
        /// <returns>Đối tượng nếu tìm thấy, null nếu không tìm thấy</returns>
        /// <exception cref="Exception">Ném ngoại lệ nếu có lỗi khi lấy dữ liệu</exception>
        /// Created by: DGKhiem (09/12/2025)
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

        /// <summary>
        /// Cập nhật thông tin của một đối tượng.
        /// </summary>
        /// <param name="entity">Đối tượng cần cập nhật</param>
        /// <returns>Đối tượng đã được cập nhật</returns>
        /// <exception cref="Exception">Ném ngoại lệ nếu có lỗi khi cập nhật dữ liệu</exception>
        /// Created by: DGKhiem (09/12/2025)
        public T Update(T entity)
        {
            try
            {
                var res = _repository.Update(entity);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating entity", ex);
            }
        }
    }
}
