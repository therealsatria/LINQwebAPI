namespace Infrastructure.DTOs
{
    public class UpdateCategoryRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}