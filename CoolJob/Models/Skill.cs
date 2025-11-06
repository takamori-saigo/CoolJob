using System.ComponentModel.DataAnnotations;
using CoolJob.Models;

public class Skill
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } 
    

    public virtual ICollection<User> Users { get; set; }
}