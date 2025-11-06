using System.ComponentModel.DataAnnotations;

namespace CoolJob.Models;

public enum JobType
{
    [Display(Name = "Полный день")]
    FullTime,
    [Display(Name = "Частичная занятость")]
    PartTime,
    [Display(Name = "Стажировка")]
    Internship,
    [Display(Name = "Удалённая работа")]
    Remote
}