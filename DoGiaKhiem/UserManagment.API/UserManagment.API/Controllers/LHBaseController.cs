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
        protected IBaseService<TEntity> _baseService;
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
        /// <returns>HTTP 200 OK với entity tương ứng</returns>
        /// Created by: DGKhiem (09/12/2025)
        [HttpGet("id")]
        public IActionResult Get([FromQuery] Guid Id)
        {
            var data = _baseService.GetById(Id);
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
                return BadRequest("DTO không được để trống");
            }

            // Map từ DTO sang Entity
            var entity = _mapperService.MapToEntity(dto);

            // Thêm entity vào database thông qua service
            var res = _baseService.Add(entity);
            return Ok(res);
        }
    }
}
