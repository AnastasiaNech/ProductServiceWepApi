using Microsoft.EntityFrameworkCore;
using ProductServiceWepApi.BLL;
using ProductServiceWepApi.Dtos;

namespace ProductServiceWepApi.EndPoints;
public class ProductEndPoints
{
    public static void AddEndpoints(WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            // получаем конечную точку
            Endpoint endpoint = context.GetEndpoint();

            if (endpoint != null)
            {
                // получаем шаблон маршрута, который ассоциирован с конечной точкой
                var routePattern = (endpoint as Microsoft.AspNetCore.Routing.RouteEndpoint)?.RoutePattern?.RawText;

                Console.WriteLine($"Endpoint Name: {endpoint.DisplayName}");
                Console.WriteLine($"Route Pattern: {routePattern}");

                // если конечная точка определена, передаем обработку дальше
                await next();
            }
            else
            {
                Console.WriteLine("Endpoint: null");
                // если конечная точка не определена, завершаем обработку
                await context.Response.WriteAsync("Endpoint is not defined");
            }
        });
        //встраивает в конвейер обработки компонент EndpointMiddleware
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("api/v1/products", async (IProductBLL productBll, string? name) =>
        {
            var products = await productBll.GetProductsByNameAsync(name);
            return Results.Ok(products);
        });

            endpoints.MapPost("api/v1/product", async (IProductBLL productBll, ProductCreateDto productCreateDto) =>
            {
                try
                {
                    var product = await productBll.CreateAsync(productCreateDto);
                    return Results.Created($"api/v1/product", product);
                }
                catch (DbUpdateException ex)
                {
                    return Results.BadRequest(ex.InnerException?.Message);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            endpoints.MapPut("api/v1/product", async (IProductBLL productBll, ProductUpdateDto productUpdateDto) =>
            {
                try
                {
                    await productBll.UpdateAsync(productUpdateDto);
                    return Results.Ok();
                }
                catch (DbUpdateException ex)
                {
                    return Results.BadRequest(ex.InnerException?.Message);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            endpoints.MapDelete("api/v1/product/{id}", async (IProductBLL productBll, Guid id) =>
            {
                try
                {
                    await productBll.DeleteAsync(id);
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
        });
    }
}