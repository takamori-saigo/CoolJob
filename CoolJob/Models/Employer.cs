using System.ComponentModel.DataAnnotations;
using CoolJob.Models;

public class Employer
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public EmployerType Type { get; set; } 
    
    public string ContactEmail { get; set; }
    public string Description { get; set; }
    
    public virtual ICollection<Job> Jobs { get; set; }
}