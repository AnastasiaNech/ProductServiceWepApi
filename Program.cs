using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductServiceWepApi.Data;
using ProductServiceWepApi.DI;
using ProductServiceWepApi.EndPoints;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Log.Information("Starting up");

try
{
var builder = WebApplication.CreateBuilder(args);
var sqlConBuilder = new SqlConnectionStringBuilder();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DI.CreateDI(builder.Services);

sqlConBuilder.ConnectionString = builder.Configuration.GetConnectionString("SQLDbConnection");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlConBuilder.ConnectionString));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
        app.UseSwagger();
        app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseRouting();

ProductEndPoints.AddEndpoints(app);

app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}




