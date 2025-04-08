using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }
    
    [Required, EmailAddress]
    public string Email { get; set; }
    
    [Required, MinLength(6)]
    public string Password { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    public string Faculty { get; set; }
    public bool IsEmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Навигационные свойства
    public virtual ICollection<Application> Applications { get; set; }
    public virtual ICollection<Skill> Skills { get; set; }
}