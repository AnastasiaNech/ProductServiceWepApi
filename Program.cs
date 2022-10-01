using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductServiceWepApi.Data;
using ProductServiceWepApi.DI;
using ProductServiceWepApi.EndPoints;

var builder = WebApplication.CreateBuilder(args);
var sqlConBuilder = new SqlConnectionStringBuilder();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DI.CreateDI(builder.Services);

sqlConBuilder.ConnectionString = builder.Configuration.GetConnectionString("SQLDbConnection");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlConBuilder.ConnectionString));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
        app.UseSwagger();
        app.UseSwaggerUI();
}


app.UseHttpsRedirection();//тоже middleware компонент

app.UseRouting();//сопоставление запросов с конкретными адресами (маршрутизация)

ProductEndPoints.AddEndpoints(app);

app.Run();
