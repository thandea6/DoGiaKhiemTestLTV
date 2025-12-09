using Microsoft.AspNetCore.Mvc;
using UserManagment.API.Helper;
using UserManagment.Core.Dtos;
using UserManagment.Core.Entities;
using UserManagment.Core.Interfaces.Services;

namespace UserManagment.API.Controllers
{
    /// <summary>
    /// Controller quản lý các API liên quan đến User
    /// Kế thừa từ LHBaseController để sử dụng các API CRUD cơ bản
    /// </summary>
    /// Created by: DGKhiem (09/12/2025)
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : LHBaseController<Users, UserDTO>
    {
        /// <summary>
        /// Constructor khởi tạo UserController
        /// </summary>
        /// <param name="baseService">Dịch vụ cơ sở cho entity Users</param>
        /// <param name="mapperService">Dịch vụ mapper để chuyển đổi UserDTO <-> Users</param>
        /// Created by: DGKhiem (09/12/2025)
        public UserController(IBaseService<Users> baseService, IMapperService<UserDTO, Users> mapperService)
            : base(baseService, mapperService)
        {
        }

        /// <summary>
        /// API thêm người dùng mới với xác thực dữ liệu
        /// Override method Insert từ base class để thêm validation logic
        /// </summary>
        /// <param name="dto">Đối tượng UserDTO chứa thông tin người dùng cần thêm</param>
        /// <returns>
        /// HTTP 200 OK với thông tin người dùng đã được thêm, hoặc
        /// HTTP 400 Bad Request nếu dữ liệu không hợp lệ
        /// </returns>
        /// Created by: DGKhiem (09/12/2025)
        [HttpPost]
        public override IActionResult Insert([FromBody] UserDTO dto)
        {
            // Kiểm tra định dạng email
            if (!ValidationHelper.IsValidEmail(dto.EmailAddress))
            {
                return BadRequest("Định dạng email không hợp lệ.");
            }

            // Kiểm tra định dạng số điện thoại (phải là 10 chữ số)
            if (!ValidationHelper.IsValidPhoneNumber(dto.PhoneNumber))
            {
                return BadRequest("Định dạng số điện thoại không hợp lệ. Phải là 10 chữ số.");
            }

            // Gọi phương thức Insert từ base class để xử lý logic chung
            return base.Insert(dto);
        }
    }
}
