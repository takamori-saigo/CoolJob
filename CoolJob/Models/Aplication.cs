using System.ComponentModel.DataAnnotations;
using CoolJob.Models;

public class Application
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int JobId { get; set; }
    
    public DateTime ApplyDate { get; set; }
    public ApplicationStatus Status { get; set; } 
    
    public string? ResumePath { get; set; }
    public string? CoverLetter { get; set; }
    
    public virtual User User { get; set; }
    public virtual Job Job { get; set; }
}