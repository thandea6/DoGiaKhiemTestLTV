using System.Reflection;

namespace UserManagment.Core.Services
{
    /// <summary>
    /// Dịch vụ auto-mapper sử dụng Reflection để tự động map các thuộc tính
    /// Tự động map những thuộc tính có cùng tên và kiểu dữ liệu tương thích
    /// </summary>
    /// <typeparam name="TSource">Kiểu dữ liệu nguồn</typeparam>
    /// <typeparam name="TDestination">Kiểu dữ liệu đích</typeparam>
    public class AutoMapperService<TSource, TDestination>
    where TSource : class
        where TDestination : class, new()
    {
        /// <summary>
        /// Thực hiện map từ đối tượng nguồn sang đối tượng đích
        /// </summary>
        /// <param name="source">Đối tượng nguồn cần map</param>
        /// <returns>Đối tượng đích đã được map từ nguồn</returns>
        /// /// CreatedBy: DGKhiem(09/12/2025)
        public TDestination Map(TSource source)
        {
            // Nếu đối tượng nguồn là null, trả về null
            if (source == null)
                return null;

            // Tạo đối tượng đích mới
            var destination = new TDestination();

            // Lấy tất cả các thuộc tính public từ lớp nguồn
            var sourceProperties = typeof(TSource).GetProperties(
               BindingFlags.Public |
                          BindingFlags.IgnoreCase |
              BindingFlags.Instance);

            // Lấy tất cả các thuộc tính public từ lớp đích
            var destinationProperties = typeof(TDestination).GetProperties(
                  BindingFlags.Public |
              BindingFlags.IgnoreCase |
               BindingFlags.Instance);

            // Duyệt qua từng thuộc tính của nguồn
            foreach (var sourceProp in sourceProperties)
            {
                // Tìm thuộc tính có cùng tên trong đích (không phân biệt hoa/thường)
                var destProp = destinationProperties.FirstOrDefault(p =>
          p.Name.Equals(sourceProp.Name, StringComparison.OrdinalIgnoreCase) &&
          p.CanWrite);

                // Nếu tìm thấy thuộc tính tương ứng và có thể đọc giá trị từ nguồn
                if (destProp != null && sourceProp.CanRead)
                {
                    try
                    {
                        // Lấy giá trị từ thuộc tính nguồn
                        var value = sourceProp.GetValue(source);

                        // Gán giá trị vào thuộc tính đích
                        destProp.SetValue(destination, value);
                    }
                    catch
                    {
                        // Bỏ qua những thuộc tính không thể map được
                    }
                }
            }

            return destination;
        }
    }
}
