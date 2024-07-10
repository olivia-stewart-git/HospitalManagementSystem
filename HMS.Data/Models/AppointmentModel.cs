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
    public DateTime APT_AppointmentTimeUTC { get; set; }

    [ForeignKey("DCT_PK")]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public DoctorModel APT_Doctor { get; set; }

    [ForeignKey("PAT_PK")]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public PatientModel APT_Patient { get; set; }

}