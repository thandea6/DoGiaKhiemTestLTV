using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagment.Core.Entities;
using UserManagment.Core.Interfaces.Repositories;
using UserManagment.Core.Interfaces.Services;

namespace UserManagment.Core.Services
{
    public class UserService : BaseService<Users>, IUserService
    {
        /// <summary>
        /// Constructor sử dụng Dependency Injection
        /// </summary>
        /// <param name="repository"></param>
        /// Created by: DGKhiem (09/12/2025)
        public UserService(IUserRepository repository) : base(repository)
        {
        }
    }
}
