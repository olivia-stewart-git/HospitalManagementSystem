using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Models;

[PrimaryKey("PAT_PK")]
public class PatientModel
{
	[Key]
	public Guid PAT_PK { get; set; }

	[ForeignKey("USR_ID")]
	public UserModel PAT_User { get; set; }
}