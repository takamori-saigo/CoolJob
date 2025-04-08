using System.ComponentModel.DataAnnotations;
using CoolJob.Models;

public class Skill
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } // Например: "C#", "Python", "UI/UX Design"
    
    // Навигационные свойства
    public virtual ICollection<User> Users { get; set; }
}