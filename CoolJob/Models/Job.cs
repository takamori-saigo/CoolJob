namespace CoolJob.Modelspublic class Job
                        {
                            public int Id { get; set; }
                            
                            [Required, StringLength(100)]
                            public string Title { get; set; }
                            
                            [Required]
                            public string Description { get; set; }
                            
                            public string Location { get; set; }
                            public decimal? Salary { get; set; }
                            
                            [Required]
                            public JobType Type { get; set; } // Enum: FullTime, PartTime, Internship
                            
                            public DateTime PostDate { get; set; }
                            public int EmployerId { get; set; }
                            
                            // Навигационные свойства
                            public virtual Employer Employer { get; set; }
                            public virtual ICollection<Application> Applications { get; set; }
                        };

public class Job
{
    
}