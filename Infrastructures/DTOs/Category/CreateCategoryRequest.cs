namespace Infrastructure;

public class CreateCategoryRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}