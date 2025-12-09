using UserManagment.Core.Interfaces.Services;

namespace UserManagment.Core.Services
{
    /// <summary>
    /// Lớp cơ sở dùng để tạo mapper với khả năng tự động map
    /// Các lớp kế thừa chỉ cần override khi có logic đặc biệt
    /// </summary>
    /// <typeparam name="TDto">Kiểu dữ liệu DTO</typeparam>
    /// <typeparam name="TEntity">Kiểu dữ liệu Entity</typeparam>
    /// CreatedBy: DGKhiem(09/12/2025)
    public abstract class BaseMapperService<TDto, TEntity> : IMapperService<TDto, TEntity>
        where TDto : class, new()
        where TEntity : class, new()
    {
        /// <summary>
        /// Map từ DTO sang Entity bằng auto-mapper
        /// </summary>
        /// <param name="dto">Đối tượng DTO cần map</param>
        /// <returns>Đối tượng Entity đã được map</returns>
        /// /// CreatedBy: DGKhiem(09/12/2025)
        protected virtual TEntity MapDtoToEntity(TDto dto)
        {
            var autoMapper = new AutoMapperService<TDto, TEntity>();
            return autoMapper.Map(dto);
        }

        /// <summary>
        /// Map từ Entity sang DTO bằng auto-mapper
        /// </summary>
        /// <param name="entity">Đối tượng Entity cần map</param>
        /// <returns>Đối tượng DTO đã được map</returns>
        /// /// CreatedBy: DGKhiem(09/12/2025)
        protected virtual TDto MapEntityToDto(TEntity entity)
        {
            var autoMapper = new AutoMapperService<TEntity, TDto>();
            return autoMapper.Map(entity);
        }

        /// <summary>
        /// Thực hiện map từ DTO sang Entity
        /// </summary>
        /// /// CreatedBy: DGKhiem(09/12/2025)
        public virtual TEntity MapToEntity(TDto dto)
        {
            return MapDtoToEntity(dto);
        }

        /// <summary>
        /// Thực hiện map từ Entity sang DTO
        /// </summary>
        /// /// CreatedBy: DGKhiem(09/12/2025)
        public virtual TDto MapToDto(TEntity entity)
        {
            return MapEntityToDto(entity);
        }
    }
}
