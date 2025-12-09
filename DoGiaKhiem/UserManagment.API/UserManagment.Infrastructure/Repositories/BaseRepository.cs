using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserManagment.Core.Interfaces.Repositories;

namespace UserManagment.Infrastructure.Repositories
{
    /// <summary>
    /// Lớp repository cơ sở thực hiện các thao tác CRUD với database sử dụng Dapper
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu Entity</typeparam>
    /// Created by: DGKhiem (08/12/2025)
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Chuỗi kết nối đến database
        /// </summary>
        protected readonly string _connectionString;

        /// <summary>
        /// Đối tượng kết nối database
        /// </summary>
        protected IDbConnection dbConnection;

        /// <summary>
        /// Tên bảng trong database
        /// </summary>
        protected readonly string _tableName;

        /// <summary>
        /// Constructor khởi tạo repository
        /// </summary>
        /// <param name="config">Configuration để lấy connection string</param>
        /// Created by: DGKhiem (08/12/2025)
        public BaseRepository(IConfiguration config)
        {
            // Lấy chuỗi kết nối từ cấu hình
            _connectionString = config.GetConnectionString("LHConnection");
            // Khởi tạo SqlConnection
            dbConnection = new SqlConnection(_connectionString);

            // Lấy tên bảng từ thuộc tính TableAttribute của lớp T (nếu có)
            var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
            _tableName = tableAttr != null ? tableAttr.Name : typeof(T).Name.ToLower();
        }

        /// <summary>
        /// Lấy tất cả bản ghi từ database
        /// </summary>
        /// <returns>Danh sách tất cả các bản ghi</returns>
        /// Created by: DGKhiem (08/12/2025)
        public IEnumerable<T> GetAll()
        {
            try
            {
                var sql = $"SELECT * FROM {_tableName}";
                var result = dbConnection.Query<T>(sql);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving data", ex);
            }
        }

        /// <summary>
        /// Lấy bản ghi theo ID từ database
        /// </summary>
        /// <param name="entityId">ID của bản ghi cần lấy</param>
        /// <returns>Bản ghi tương ứng với ID</returns>
        /// Created by: DGKhiem (08/12/2025)
        public T GetById(Guid entityId)
        {
            try
            {
                // Lấy tên bảng từ TableAttribute
                var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;
                var properties = typeof(T).GetProperties();

                // Tìm thuộc tính có KeyAttribute (khóa chính)
                var columnProperties = typeof(T).GetProperties()
                                .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
                var keyColumnName = columnProperties.GetCustomAttribute<ColumnAttribute>().Name;

                // Tạo câu lệnh SQL và parameters
                var parameters = new DynamicParameters();
                var sql = $"SELECT * FROM {tableName} WHERE {keyColumnName} = @Id";
                parameters.Add("@Id", entityId);

                // Thực thi query
                var data = dbConnection.QuerySingleOrDefault<T>(sql, parameters);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving data by ID: {entityId}", ex);
            }
        }

        /// <summary>
        /// Thêm bản ghi mới vào database
        /// </summary>
        /// <param name="entity">Bản ghi cần thêm</param>
        /// <returns>Bản ghi đã được thêm</returns>
        /// Created by: DGKhiem (08/12/2025)
        public T Add(T entity)
        {
            try
            {
                var properties = typeof(T).GetProperties();
                var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;
                var columns = "";
                var columnParam = "";
                var parameters = new DynamicParameters();

                // Tự động tạo GUID cho thuộc tính ID nếu có
                foreach (var prop in properties)
                {
                    if (prop.PropertyType == typeof(Guid) && prop.Name == $"{typeof(T).Name}Id")
                    {
                        prop.SetValue(entity, Guid.NewGuid());
                    }
                }

                // Xây dựng câu lệnh INSERT với tất cả các cột
                foreach (var prop in properties)
                {
                    var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
                    if (columnAttr == null) continue;

                    var columnName = columnAttr.Name;
                    columns += $"{columnName},";
                    columnParam += $"@{prop.Name},";
                    parameters.Add($"@{prop.Name}", prop.GetValue(entity));
                }

                columns = columns.TrimEnd(',');
                columnParam = columnParam.TrimEnd(',');

                // Thực thi câu lệnh INSERT
                var sql = @$"INSERT INTO {tableName} ({columns}) VALUES ({columnParam})";
                var res = dbConnection.Execute(sql, parameters);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting data", ex);
            }
        }

        /// <summary>
        /// Cập nhật bản ghi trong database
        /// </summary>
        /// <param name="entity">Bản ghi cần cập nhật</param>
        /// <returns>Bản ghi đã được cập nhật</returns>
        /// Created by: DGKhiem (08/12/2025)
        public T Update(T entity)
        {
            try
            {
                var properties = typeof(T).GetProperties();
                var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
                var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;
                var column = "";
                var parameters = new DynamicParameters();

                // Tìm thuộc tính khóa chính
                var keyProp = properties.FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
                var keyColumnName = keyProp.GetCustomAttribute<ColumnAttribute>().Name;

                // Xây dựng câu lệnh UPDATE (bỏ qua khóa chính)
                foreach (var prop in properties)
                {
                    if (prop == keyProp) continue;

                    var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
                    if (columnAttr == null) continue;

                    var columnName = columnAttr.Name;
                    column += $"{columnName} = @{prop.Name},";
                    parameters.Add($"@{prop.Name}", prop.GetValue(entity));
                }

                column = column.TrimEnd(',');
                parameters.Add($"@{keyProp.Name}", keyProp.GetValue(entity));

                // Thực thi câu lệnh UPDATE
                var sql = @$"UPDATE {tableName} SET {column} WHERE {keyColumnName} = @{keyProp.Name};";
                var res = dbConnection.Execute(sql, parameters);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating data", ex);
            }
        }

        /// <summary>
        /// Xóa bản ghi theo ID từ database
        /// </summary>
        /// <param name="entityId">ID của bản ghi cần xóa</param>
        /// Created by: DGKhiem (08/12/2025)
        public void Delete(Guid entityId)
        {
            try
            {
                var properties = typeof(T).GetProperties();
                var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;

                // Tìm thuộc tính khóa chính
                var keyProp = properties.FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
                var keyColumnName = keyProp.GetCustomAttribute<ColumnAttribute>().Name;

                // Thực thi câu lệnh DELETE
                var sql = @$"DELETE FROM {tableName} WHERE {keyColumnName} = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", entityId);
                var res = dbConnection.Execute(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting data", ex);
            }
        }
    }
}
