namespace UserManagementApp.FE.Models
{
    /// <summary>
    /// Model cho input form
    /// </summary>
    /// Created by: DGKhiem (09/12/2025)
    public class UserInputModel
    {
        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// ngày sinh
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
