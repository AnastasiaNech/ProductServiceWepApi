using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductServiceWepApi.Models;

[Table("Product")]
public class Product
{
    [Key]
    public Guid ID { get; set; }

    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }

    public string? Description { get; set; }

}
