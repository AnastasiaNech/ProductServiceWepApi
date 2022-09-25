using Microsoft.EntityFrameworkCore;
using ProductServiceWepApi.Models;

namespace ProductServiceWepApi.Data;

public class ProductRepo: IProductRepo
{
    private readonly AppDbContext _context;

    public ProductRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        name = name ?? String.Empty;

        var list = await _context.Product.ToListAsync();

        var searchingName = name.Trim().ToLower();

        return list.Where(c => c.Name.ToLower().Contains(searchingName));
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _context.Product.FirstOrDefaultAsync(c => c.ID == id);
    }

    public async Task CreateProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentNullException.ThrowIfNull(product.Name);       
        await _context.AddAsync(product);
    }

    public void DeleteProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        _context.Product.Remove(product);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
