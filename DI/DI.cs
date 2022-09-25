using ProductServiceWepApi.BLL;
using ProductServiceWepApi.Data;

namespace ProductServiceWepApi.DI;

public class DI
{
    public static void CreateDI(IServiceCollection services)
    {
        services.AddScoped<IProductRepo, ProductRepo>();
        services.AddScoped<IProductBLL, ProductBLL>();
    }
}
