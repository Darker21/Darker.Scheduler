using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Darker.Scheduler.Data.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    public int ScheduleId { get; set; }
    [ForeignKey("ScheduleId")]
    public Schedule Schedule { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string Details { get; set; } = ""; // JSON or text
}
