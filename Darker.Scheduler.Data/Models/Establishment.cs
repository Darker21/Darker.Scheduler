using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Darker.Scheduler.Data.Models;

public class Establishment
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public EstablishmentType Type { get; set; } = EstablishmentType.Unspecified; // "Bar" or "Restaurant"
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}

public enum EstablishmentType
{
    Unspecified,
    Bar,
    Restaurant
}
