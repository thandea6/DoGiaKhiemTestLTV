using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagment.Core.Entities
{
    /// <summary>
    /// thực thể người dùng
    /// </summary>
    /// CreatedBy: DGKhiem(08/12/2025)
    [Table("Users")]
    public class User
    {
        /// <summary>
        /// Id nguời dùng
        /// </summary>
        [Key]
        [Column("user_id")]
        public Guid UserId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// tên người dùng
        /// </summary>
        [Column("full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// ngày sinh
        /// </summary>
        [Column("birth_date")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// địa chỉ email
        /// </summary>
        [Column("email_address")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// số điện thoại
        /// </summary>
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// địa chỉ
        /// </summary>
        [Column("address")]
        public string Address { get; set; }
    }
}
