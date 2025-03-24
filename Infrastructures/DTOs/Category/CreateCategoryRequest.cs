using System.ComponentModel.DataAnnotations;
using Infrastructure.DTOs.Generic;

namespace Infrastructure.DTOs.Category
{
    public class CreateCategoryRequest : CreateRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}