using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using CoolJob.Models;

public class Job
{
    public int Id { get; set; }
    
    [Required, StringLength(100)]
    public string Title { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    public string Location { get; set; }
    public decimal? Salary { get; set; }
    
    [Required]
    [Display(Name = "Тип занятости")]
    public JobType Type { get; set; } 
    [Display(Name = "Дата публикации")]
    
    public DateTime PostDate { get; set; }
    public int EmployerId { get; set; }
    
    public virtual Employer Employer { get; set; }
    public virtual ICollection<Application> Applications { get; set; }
    [Display(Name = "Активна")]
    public bool IsActive { get; set; } = true;
}