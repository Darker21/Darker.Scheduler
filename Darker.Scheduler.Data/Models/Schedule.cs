using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Darker.Scheduler.Data.Models;

public class Schedule
{
    [Key]
    public int Id { get; set; }
    public int EstablishmentId { get; set; }
    [ForeignKey("EstablishmentId")]
    public Establishment Establishment { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string CronExpression { get; set; } = string.Empty;
    public int MaxDaysInAdvance { get; set; }
    public string Configuration { get; set; } = "{}"; // JSON
    public bool IsActive { get; set; } = true;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
