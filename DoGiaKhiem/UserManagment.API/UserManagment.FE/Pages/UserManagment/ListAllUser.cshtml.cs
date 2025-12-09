using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementApp.FE.Models;
using System.Text.Json;

namespace UserManagementApp.FE.Pages.UserManagment
{
    /// <summary>
    /// PageModel cho trang danh sách ng??i dùng
    /// </summary>
    /// Created by: DGKhiem (09/12/2025)
    public class ListAllUserModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Danh sách người dùng
        /// </summary>
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();

        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// URL base của API
        /// </summary>
        public string ApiBaseUrl { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ListAllUserModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            ApiBaseUrl = _configuration["ApiSettings:BaseUrl"];
        }

        /// <summary>
        /// Xử lý khi trang được load
        /// </summary>
        /// Created by: DGKhiem (09/12/2025)
        public async Task OnGetAsync()
        {
            try
            {
                // Tạo HTTP client
                var httpClient = _httpClientFactory.CreateClient();

                // Gọi API GetAll
                var response = await httpClient.GetAsync($"{ApiBaseUrl}/User");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Users = JsonSerializer.Deserialize<List<UserViewModel>>(content, options) ?? new List<UserViewModel>();
                }
                else
                {
                    ErrorMessage = $"Lỗi khi tải danh sách người dùng. Mã lỗi: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi: {ex.Message}";
            }
        }
    }
}
