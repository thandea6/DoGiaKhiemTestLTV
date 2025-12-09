using Microsoft.AspNetCore.Mvc;
using UserManagment.Core.Interfaces.Services;

namespace UserManagment.API.Controllers
{
    /// <summary>
    /// Base controller generic hỗ trợ DTO với khả năng tự động map
    /// Sử dụng cho các controller muốn nhận DTO thay vì Entity trực tiếp
    /// </summary>
    /// <typeparam name="TEntity">Kiểu dữ liệu Entity (domain model)</typeparam>
    /// <typeparam name="TDto">Kiểu dữ liệu DTO (data transfer object)</typeparam>
    /// Created by: DGKhiem (09/12/2025)
    [Route("api/[controller]")]
    [ApiController]
    public class LHBaseController<TEntity, TDto> : ControllerBase
        where TEntity : class
        where TDto : class
    {
        /// <summary>
        /// Dịch vụ cơ sở để xử lý business logic cho Entity
        /// </summary>
        protected IBaseService<TEntity> _baseService;

        /// <summary>
        /// Dịch vụ mapper để chuyển đổi giữa DTO và Entity
        /// </summary>
        protected IMapperService<TDto, TEntity> _mapperService;

        /// <summary>
        /// Constructor khởi tạo base controller với DTO support
        /// </summary>
        /// <param name="baseService">Dịch vụ cơ sở cho entity</param>
        /// <param name="mapperService">Dịch vụ mapper để chuyển đổi DTO <-> Entity</param>
        /// Created by: DGKhiem (09/12/2025)
        public LHBaseController(IBaseService<TEntity> baseService, IMapperService<TDto, TEntity> mapperService)
        {
            _baseService = baseService;
            _mapperService = mapperService;
        }

        /// <summary>
        /// API lấy danh sách tất cả các entity
        /// </summary>
        /// <returns>HTTP 200 OK với danh sách tất cả các entity</returns>
        /// Created by: DGKhiem (09/12/2025)
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _baseService.GetAll();
            return Ok(data);
        }

        /// <summary>
        /// API lấy entity theo ID
        /// </summary>
        /// <param name="Id">ID của entity cần lấy</param>
        /// <returns>HTTP 200 OK với entity tương ứng, hoặc HTTP 404 Not Found nếu không tìm thấy</returns>
        /// Created by: DGKhiem (09/12/2025)
        [HttpGet("id")]
        public IActionResult Get([FromQuery] Guid Id)
        {
            var data = _baseService.GetById(Id);

            if (data == null)
            {
                return NotFound($"Không tìm thấy bản ghi với ID: {Id}");
            }

            return Ok(data);
        }

        /// <summary>
        /// API thêm entity mới từ DTO
        /// Tự động map từ DTO sang Entity trước khi lưu vào database
        /// </summary>
        /// <param name="dto">DTO chứa thông tin entity cần thêm</param>
        /// <returns>HTTP 200 OK với entity đã được thêm, hoặc HTTP 400 Bad Request nếu DTO null</returns>
        /// Created by: DGKhiem (09/12/2025)
        [HttpPost]
        public virtual IActionResult Insert([FromBody] TDto dto)
        {
            // Kiểm tra DTO không được null
            if (dto == null)
            {
                return BadRequest("Không được để trống");
            }

            // Map từ DTO sang Entity
            var entity = _mapperService.MapToEntity(dto);

            // Thêm entity vào database thông qua service
            var res = _baseService.Add(entity);
            return Ok(res);
        }

        /// <summary>
        /// API cập nhật entity
        /// </summary>
        /// <param name="entity">Entity chứa thông tin mới cần cập nhật</param>
        /// <returns>HTTP 200 OK với entity đã được cập nhật</returns>
        /// Created by: DGKhiem (09/12/2025)
        [HttpPut]
        public virtual IActionResult Update(TEntity entity)
        {
            // Gọi dịch vụ để cập nhật dữ liệu
            var res = _baseService.Update(entity);
            return Ok(res);
        }

        /// <summary>
        /// API xóa entity theo ID
        /// </summary>
        /// <param name="id">ID của entity cần xóa</param>
        /// <returns>
        /// HTTP 200 OK với thông báo xóa thành công,
        /// HTTP 404 Not Found nếu không tìm thấy entity,
        /// HTTP 500 Internal Server Error nếu xóa thất bại
        /// </returns>
        /// Created by: DGKhiem (09/12/2025)
        [HttpDelete("{id}")]
        public virtual IActionResult Delete([FromRoute] Guid id)
        {
            // Kiểm tra entity có tồn tại không
            var exists = _baseService.GetById(id);
            if (exists == null)
            {
                return NotFound($"Không tìm thấy bản ghi với ID: {id}");
            }

            // Gọi dịch vụ để xóa dữ liệu
            var res = _baseService.Delete(id);

            if (res)
            {
                return Ok($"Xóa bản ghi với ID: {id} thành công");
            }
            else
            {
                return StatusCode(500, "Xóa bản ghi thất bại do lỗi server");
            }
        }
    }
}
