using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementApp.FE.Models;
using System.Text;
using System.Text.Json;

namespace UserManagementApp.FE.Pages.UserManagment
{
    /// <summary>
    /// PageModel cho trang cập nhật thông tin người dùng
    /// </summary>
    /// Created by: DGKhiem (09/12/2025)
    public class UpdateUserInfoModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// URL base của API
        /// </summary>
        public string ApiBaseUrl { get; set; }

        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Thông báo thành công
        /// </summary>
        [TempData]
        public string SuccessMessage { get; set; }

        /// <summary>
        /// User input data
        /// </summary>
        [BindProperty]
        public UserUpdateModel UserData { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdateUserInfoModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            ApiBaseUrl = _configuration["ApiSettings:BaseUrl"];
        }

        /// <summary>
        /// Xử lý khi trang được load (GET) - Lấy thông tin user theo ID
        /// </summary>
        /// <param name="id">ID của user cần cập nhật</param>
        /// Created by: DGKhiem (09/12/2025)
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                // Tạo HTTP client
                var httpClient = _httpClientFactory.CreateClient();

                // Gọi API GET user by ID
                var response = await httpClient.GetAsync($"{ApiBaseUrl}/User/id?Id={id}");

                if (response.IsSuccessStatusCode)
                {
                    // Đọc response content
                    var content = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON thành user object
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var user = JsonSerializer.Deserialize<UserViewModel>(content, options);

                    if (user != null)
                    {
                        // Map sang UserUpdateModel
                        UserData = new UserUpdateModel
                        {
                            UserId = user.UserId,
                            FullName = user.FullName,
                            BirthDate = user.BirthDate ?? DateTime.Now,
                            EmailAddress = user.EmailAddress,
                            PhoneNumber = user.PhoneNumber,
                            Address = user.Address
                        };

                        return Page();
                    }
                    else
                    {
                        ErrorMessage = "Không tìm thấy người dùng";
                        return RedirectToPage("/UserManagment/ListAllUser");
                    }
                }
                else
                {
                    ErrorMessage = $"Lỗi khi tải thông tin người dùng. Mã lỗi: {response.StatusCode}";
                    return RedirectToPage("/UserManagment/ListAllUser");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi: {ex.Message}";
                return RedirectToPage("/UserManagment/ListAllUser");
            }
        }

        /// <summary>
        /// Xử lý khi form được submit (POST) - Cập nhật thông tin user
        /// </summary>
        /// Created by: DGKhiem (09/12/2025)
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Tạo HTTP client
                var httpClient = _httpClientFactory.CreateClient();

                // Tạo entity để gửi lên API (PUT nhận Entity, không phải DTO)
                var userEntity = new
                {
                    userId = UserData.UserId,
                    fullName = UserData.FullName,
                    birthDate = UserData.BirthDate,
                    emailAddress = UserData.EmailAddress,
                    phoneNumber = UserData.PhoneNumber,
                    address = UserData.Address
                };

                // Serialize object thành JSON
                var jsonContent = JsonSerializer.Serialize(userEntity);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gọi API PUT
                var response = await httpClient.PutAsync($"{ApiBaseUrl}/User", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = "Cập nhật thông tin người dùng thành công!";
                    return RedirectToPage("/UserManagment/ListAllUser");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Lỗi khi cập nhật: {errorContent}";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi: {ex.Message}";
                return Page();
            }
        }
    }
}
