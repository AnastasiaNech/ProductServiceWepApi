namespace ProductServiceWepApi.Dtos;

public class ProductReadDto

{
    public Guid ID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
