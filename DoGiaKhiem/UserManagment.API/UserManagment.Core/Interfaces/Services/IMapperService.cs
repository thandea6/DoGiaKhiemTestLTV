namespace UserManagment.Core.Interfaces.Services
{
    /// <summary>
    /// Interface dùng để map dữ liệu giữa DTO và Entity
    /// </summary>
    /// <typeparam name="TDto">Kiểu dữ liệu DTO</typeparam>
    /// <typeparam name="TEntity">Kiểu dữ liệu Entity</typeparam>
    /// /// CreatedBy: DGKhiem(09/12/2025)
    public interface IMapperService<TDto, TEntity>
        where TDto : class
        where TEntity : class
    {
        /// <summary>
        /// Map từ DTO sang Entity
        /// </summary>
        /// <param name="dto">Đối tượng DTO cần map</param>
        /// <returns>Đối tượng Entity đã được map</returns>
        /// /// CreatedBy: DGKhiem(09/12/2025)
        TEntity MapToEntity(TDto dto);

        /// <summary>
        /// Map từ Entity sang DTO
        /// </summary>
        /// <param name="entity">Đối tượng Entity cần map</param>
        /// <returns>Đối tượng DTO đã được map</returns>
        /// /// CreatedBy: DGKhiem(09/12/2025)
        TDto MapToDto(TEntity entity);
    }
}
