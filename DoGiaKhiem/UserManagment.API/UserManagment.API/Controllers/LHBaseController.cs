using Microsoft.AspNetCore.Mvc;
using UserManagment.Core.Interfaces.Services;

namespace UserManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LHBaseController<T> : ControllerBase where T : class
    {
        private IBaseService<T> _baseService;

        public LHBaseController(IBaseService<T> baseService)
        {
            _baseService = baseService;
        }

        [HttpGet]
        public IActionResult Get()
        {
                var data = _baseService.GetAll();

                return Ok(data);
        }
    }
}
