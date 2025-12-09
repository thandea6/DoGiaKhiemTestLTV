using System.Text.RegularExpressions;

namespace UserManagment.API.Helper
{
    public class ValidationHelper
    {
        /// <summary>
        /// Check định dạng email hợp lệ
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// Created by: DGKhiem (09/12/2025)
        public static bool IsValidEmail(string email)
        {
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }

        /// <summary>
        /// Check định dạng số điện thoại hợp lệ
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        /// Created by: DGKhiem (09/12/2025)
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            var phonePattern = @"^\d{10}$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }
    }
}
