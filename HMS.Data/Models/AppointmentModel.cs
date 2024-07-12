using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Models;

[PrimaryKey("APT_PK")]
public class AppointmentModel
{
    [Key]
    public Guid APT_PK { get; set; }

    [Required]
    public DateTime APT_AppointmentTime { get; set; }

    [ForeignKey("APT_DCT_ID")]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public DoctorModel APT_Doctor { get; set; }
    public Guid APT_DCT_ID { get; set; }

    [ForeignKey("APT_PAT_ID")]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public PatientModel APT_Patient { get; set; }
    public Guid APT_PAT_ID { get; set; }

    [MaxLength(400)]
    public string APT_Description { get; set; } = string.Empty;
}