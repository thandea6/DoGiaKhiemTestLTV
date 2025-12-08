using Microsoft.AspNetCore.Mvc;
using UserManagment.Core.Entities;
using UserManagment.Core.Interfaces.Services;

namespace UserManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : LHBaseController<User>
    {
        public UserController(IBaseService<User> baseService) : base(baseService)
        {
        }
    }
}
