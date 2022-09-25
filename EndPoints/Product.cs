using Microsoft.EntityFrameworkCore;
using ProductServiceWepApi.BLL;
using ProductServiceWepApi.Dtos;

namespace ProductServiceWepApi.EndPoints;
public class ProductEndPoints
{
    public static void AddEndpoints(WebApplication app)
    {
        app.MapGet("api/v1/products", async (IProductBLL productBll, string? name) =>
        {
            var products = await productBll.GetProductsByNameAsync(name);
            return Results.Ok(products);
        });

        app.MapPost("api/v1/product", async (IProductBLL productBll, ProductCreateDto productCreateDto) =>
        {
            try
            {
                var product = await productBll.CreateAsync(productCreateDto);
                return Results.Created($"api/v1/command/{product.ID}", product);
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

        app.MapPut("api/v1/product/{id}", async (IProductBLL productBll, ProductUpdateDto productUpdateDto) =>
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

        app.MapDelete("api/v1/product/{id}", async (IProductBLL productBll, Guid id) =>
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
    }
}