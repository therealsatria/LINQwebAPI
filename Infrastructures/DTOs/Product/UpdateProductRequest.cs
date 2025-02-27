using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs;

public class UpdateProductRequest
{
    [Required]
    public string Name {get; set;} = string.Empty;
    [Required]
    public string Description {get; set;} = string.Empty;
    [Required]
    public decimal Price {get; set;}
    [Required]
    public Guid CategoryId {get; set;}
}