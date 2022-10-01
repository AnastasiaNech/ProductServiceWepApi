using ProductServiceWepApi.Dtos;

namespace ProductServiceWepApi.BLL;
public interface IProductBLL
{
    Task<List<ProductReadDto>> GetProductsByNameAsync(string name);
    Task<ProductReadDto> CreateAsync(ProductCreateDto obj);
    Task UpdateAsync(ProductUpdateDto obj);
    Task DeleteAsync(Guid id);
}
