using AutoMapper;
using ProductServiceWepApi.Data;
using ProductServiceWepApi.Dtos;
using ProductServiceWepApi.Models;

namespace ProductServiceWepApi.BLL;
public class ProductBLL : IProductBLL
{
    private readonly IMapper _mapper;
    private readonly IProductRepo _repo;
    public ProductBLL(
        IMapper mapper,
        IProductRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<IEnumerable<ProductReadDto>> GetProductsByNameAsync(string name)
    {
        var products = await _repo.GetProductsByNameAsync(name);
        return _mapper.Map<IEnumerable<ProductReadDto>>(products);
    }

    public async Task<ProductReadDto> CreateAsync(ProductCreateDto obj)
    {
        this.CheckName(obj.Name);

        var productModel = _mapper.Map<Product>(obj);
        await _repo.CreateProductAsync(productModel);
        await _repo.SaveChangesAsync();
        return _mapper.Map<ProductReadDto>(productModel);
    }

    public async Task UpdateAsync(ProductUpdateDto obj)
    {
        this.CheckName(obj.Name);

        var product = await _repo.GetProductByIdAsync(obj.ID);

        ArgumentNullException.ThrowIfNull(product);

        _mapper.Map(obj, product);
        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _repo.GetProductByIdAsync(id);

        ArgumentNullException.ThrowIfNull(product);

        _repo.DeleteProduct(product);
        await _repo.SaveChangesAsync();
    }

    private void CheckName(string? name)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        };
    }
}
