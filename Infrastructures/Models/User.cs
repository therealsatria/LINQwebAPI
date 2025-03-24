using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    
    [Required]
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    
    public string Role { get; set; } = "User"; // Default role
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 