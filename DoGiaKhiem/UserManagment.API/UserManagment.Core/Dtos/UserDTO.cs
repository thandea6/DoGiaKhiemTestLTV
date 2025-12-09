using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagment.Core.Dtos
{
    public class UserDTO
    {
        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Ngày sinh (format: dd-MM-yyyy)
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// địa chỉ email
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// địa chỉ
        /// </summary>
        public string Address { get; set; }
    }
}
