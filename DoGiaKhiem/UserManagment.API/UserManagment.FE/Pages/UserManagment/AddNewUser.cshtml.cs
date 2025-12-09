using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementApp.FE.Models;
using System.Text;
using System.Text.Json;

namespace UserManagementApp.FE.Pages.UserManagment
{
    /// <summary>
    /// PageModel cho trang thêm người dùng mới
    /// </summary>
    /// Created by: DGKhiem (09/12/2025)
    public class AddNewUserModel : PageModel
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
        public UserInputModel UserInput { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AddNewUserModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            ApiBaseUrl = _configuration["ApiSettings:BaseUrl"];
        }

        /// <summary>
        /// Xử lý khi trang được load (GET)
        /// </summary>
        /// Created by: DGKhiem (09/12/2025)
        public void OnGet()
        {
            // Initialize empty form
        }

        /// <summary>
        /// Xử lý khi form được submit (POST)
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

                // Tạo DTO để gửi lên API
                var userDto = new
                {
                    fullName = UserInput.FullName,
                    birthDate = UserInput.BirthDate,
                    emailAddress = UserInput.EmailAddress,
                    phoneNumber = UserInput.PhoneNumber,
                    address = UserInput.Address
                };

                // Serialize object thành JSON
                var jsonContent = JsonSerializer.Serialize(userDto);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gọi API POST
                var response = await httpClient.PostAsync($"{ApiBaseUrl}/User", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = "Thêm người dùng thành công!";
                    return RedirectToPage("/UserManagment/ListAllUser");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Lỗi khi thêm người dùng: {errorContent}";
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
