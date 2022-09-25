using System.ComponentModel.DataAnnotations;

namespace ProductServiceWepApi.Dtos;

public class ProductUpdateDto
{
    public Guid ID { get; set; }
    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }
    public string? Description { get; set; }
}
