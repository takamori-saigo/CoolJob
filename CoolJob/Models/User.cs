using System.ComponentModel.DataAnnotations;

namespace CoolJob.Models;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    [Display(Name = "Электронная почта")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Пароль обязателен")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
    [Display(Name = "Пароль")]
    public string Password { get; set; }
    
    [Required]
    [Display(Name = "Имя")]
    public string FirstName { get; set; }
    
    [Required]
    [Display(Name = "Фамилия")]
    public string LastName { get; set; }
    
    public string? Faculty { get; set; }
    public bool IsEmailVerified { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    

    public virtual ICollection<Application> Applications { get; set; }
    public virtual ICollection<Skill> Skills { get; set; }
}