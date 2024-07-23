using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HMS.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Models;

[PrimaryKey("DCT_PK")]
public class DoctorModel : IDbModel, IUser
{
	[Key]
	public Guid DCT_PK { get; set; }

	[ForeignKey("DCT_USR_ID")]
	public UserModel DCT_User { get; set; }
	public Guid DCT_USR_ID { get; set; }

	[InverseProperty(nameof(AppointmentModel.APT_Doctor))]
	public ICollection<AppointmentModel> DCT_Appointments { get; set; } = [];

	[InverseProperty(nameof(PatientModel.PAT_Doctor))]
	public ICollection<PatientModel> DCT_Patients { get; set; } = [];

	[NotMapped]
    public UserModel User { get => DCT_User; set => DCT_User = value; }
}