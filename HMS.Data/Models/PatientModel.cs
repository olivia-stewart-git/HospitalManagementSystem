using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HMS.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Models;

[PrimaryKey("PAT_PK")]
public class PatientModel : IDbModel, IUser
{
	[Key]
	public Guid PAT_PK { get; set; }

	[ForeignKey("PAT_USR_ID")]
	public UserModel PAT_User { get; set; }
	public Guid PAT_USR_ID { get; set; }

	[InverseProperty(nameof(AppointmentModel.APT_Patient))]
	public ICollection<AppointmentModel> PAT_Appointments { get; set; } = [];

	[DeleteBehavior(DeleteBehavior.NoAction)]
	[ForeignKey("PAT_DCT_ID")]
	public DoctorModel? PAT_Doctor { get; set; }
	public Guid PAT_DCT_ID;

	[NotMapped]
    public UserModel User { get => PAT_User; set => PAT_User = value; }
}