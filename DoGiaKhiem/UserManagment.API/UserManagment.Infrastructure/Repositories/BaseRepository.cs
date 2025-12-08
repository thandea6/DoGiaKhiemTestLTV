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
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly string _connectionString;
        protected IDbConnection dbConnection;

        protected readonly string _tableName;

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

        public IEnumerable<T> GetAll()
        {
            try
            {
                var sql = $"SELECT * FROM {_tableName}";
                var result = dbConnection.Query<T>(sql);

                Console.WriteLine("Dữ liệu trả về từ cơ sở dữ liệu:");
                foreach (var item in result)
                {
                    Console.WriteLine($"ID: {item.GetType().GetProperty("UserId")?.GetValue(item)}, Name: {item.GetType().GetProperty("FullName")?.GetValue(item)}");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving data", ex);
            }
        }

        public T GetById(Guid entityId)
        {
            try
            {
                var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;
                var properties = typeof(T).GetProperties();
                var columnProperties = typeof(T).GetProperties()
                                .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
                var keyColumnName = columnProperties.GetCustomAttribute<ColumnAttribute>().Name;
                var parameters = new DynamicParameters();
                var sql = $"SELECT * FROM {tableName} WHERE {keyColumnName} = @Id";
                parameters.Add("@Id", entityId);
                var data = dbConnection.QuerySingleOrDefault<T>(sql, parameters);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving data by ID: {entityId}", ex);
            }
        }

        public T Add(T entity)
        {
            try
            {
                var properties = typeof(T).GetProperties();
                var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;
                var columns = "";
                var columnParam = "";
                var parameters = new DynamicParameters();

                foreach (var prop in properties)
                {
                    if (prop.PropertyType == typeof(Guid) && prop.Name == $"{typeof(T).Name}Id")
                    {
                        prop.SetValue(entity, Guid.NewGuid());
                    }
                }

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
                var sql = @$"INSERT INTO {tableName} ({columns}) VALUES ({columnParam})";
                var res = dbConnection.Execute(sql, parameters);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting data", ex);
            }
        }

        public T Update(T entity)
        {
            try
            {
                var properties = typeof(T).GetProperties();
                var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
                var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;
                var column = "";
                var parameters = new DynamicParameters();
                var keyProp = properties.FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
                var keyColumnName = keyProp.GetCustomAttribute<ColumnAttribute>().Name;

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
                var sql = @$"UPDATE {tableName} SET {column} WHERE {keyColumnName} = @{keyProp.Name};";
                var res = dbConnection.Execute(sql, parameters);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating data", ex);
            }
        }

        public void Delete(Guid entityId)
        {
            try
            {
                var properties = typeof(T).GetProperties();
                var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;
                var keyProp = properties.FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
                var keyColumnName = keyProp.GetCustomAttribute<ColumnAttribute>().Name;
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
