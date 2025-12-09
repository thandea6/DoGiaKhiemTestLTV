using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagment.Core.Interfaces.Services
{
    /// <summary>
    /// Interface dịch vụ cơ sở cho các thao tác CRUD
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu Entity</typeparam>
    /// Created by: DGKhiem (09/12/2025)
    public interface IBaseService<T>
    {
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách tất cả các bản ghi</returns>
        /// Created by: DGKhiem (09/12/2025)
        IEnumerable<T> GetAll();

        /// <summary>
        /// Lấy bản ghi theo ID
        /// </summary>
        /// <param name="entityId">ID của bản ghi cần lấy</param>
        /// <returns>Bản ghi tương ứng với ID</returns>
        /// Created by: DGKhiem (09/12/2025)
        T GetById(Guid entityId);

        /// <summary>
        /// Thêm bản ghi mới
        /// </summary>
        /// <param name="entity">Bản ghi cần thêm</param>
        /// <returns>Bản ghi đã được thêm</returns>
        /// Created by: DGKhiem (09/12/2025)
        T Add(T entity);

        /// <summary>
        /// Cập nhật bản ghi
        /// </summary>
        /// <param name="entity">Bản ghi cần cập nhật</param>
        /// <returns>Bản ghi đã được cập nhật</returns>
        /// Created by: DGKhiem (09/12/2025)
        T Update(T entity);

        /// <summary>
        /// Xóa bản ghi theo ID
        /// </summary>
        /// <param name="entityId">ID của bản ghi cần xóa</param>
        /// <returns>True nếu xóa thành công, False nếu thất bại</returns>
        /// Created by: DGKhiem (09/12/2025)
        bool Delete(Guid entityId);
    }
}
