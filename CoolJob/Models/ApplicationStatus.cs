using System.ComponentModel.DataAnnotations;

namespace CoolJob.Models;

public enum ApplicationStatus
{
    [Display(Name = "На рассмотрении")]
    Pending,
    [Display(Name = "Принято")]
    Accepted,
    [Display(Name = "Отклонено")]
    Rejected,
    [Display(Name = "На собеседовании")]
    Interview
}