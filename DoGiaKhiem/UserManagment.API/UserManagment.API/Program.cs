using UserManagment.Core.Dtos;
using UserManagment.Core.Entities;
using UserManagment.Core.Interfaces.Repositories;
using UserManagment.Core.Interfaces.Services;
using UserManagment.Core.Services;
using UserManagment.Infrastructure.Mappers;
using UserManagment.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Cấu hỉnh Dapper để map các thuộc tính có dấu gạch dưới với thuộc tính trong class
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

/// <summary>
/// Add CORS policy
/// </summary>
/// Created by: DGKhiem (09/12/2025)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
  .AllowAnyMethod()
     .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Register mappers
builder.Services.AddScoped<IMapperService<UserDTO, Users>, UserMapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
