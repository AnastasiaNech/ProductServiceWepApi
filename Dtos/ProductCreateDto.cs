using System.ComponentModel.DataAnnotations;

namespace ProductServiceWepApi.Dtos;

public class ProductCreateDto
{
    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }
    public string? Description { get; set; }
}
