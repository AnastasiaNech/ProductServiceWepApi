using ProductServiceWepApi.Models;

namespace ProductServiceWepApi.Data;

public interface IProductRepo
{

    Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    Task<Product?> GetProductByIdAsync(Guid id);
    Task CreateProductAsync(Product product);
    void DeleteProduct(Product product);
    Task SaveChangesAsync();
}
