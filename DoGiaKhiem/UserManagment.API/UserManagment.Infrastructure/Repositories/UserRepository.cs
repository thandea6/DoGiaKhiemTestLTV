using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagment.Core.Entities;
using UserManagment.Core.Interfaces.Repositories;

namespace UserManagment.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<Users>, IUserRepository
    {
        /// <summary>
        /// khởi tạo UserRepository
        /// </summary>
        /// <param name="config"></param>
        public UserRepository(IConfiguration config) : base(config)
        {
        }
    }
}
