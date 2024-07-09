using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Models;

[PrimaryKey("APT_Model")]
public class AppointmentModel
{
    [Key]
    public Guid APT_Model { get; set; }
    [Required]
    public DateTime APT_AppointmentTimeUTC { get; set; }

    [ForeignKey("DCT_PK")]
    public DoctorModel APT_Doctor { get; set; }

    [ForeignKey("PAT_PK")]
    public DoctorModel APT_Patient { get; set; }

}