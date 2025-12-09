namespace UserManagementApp.FE.Models
{
    /// <summary>
    /// Model User ?? hi?n th? trên FE
    /// </summary>
    /// Created by: DGKhiem (09/12/2025)
    public class UserViewModel
    {
        /// <summary>
        /// ID ng??i dùng
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Tên ??y ??
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// ??a ch? email
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// S? ?i?n tho?i
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// ??a ch?
        /// </summary>
        public string Address { get; set; }
    }
}
