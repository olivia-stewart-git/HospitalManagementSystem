using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HMS.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Models;

[PrimaryKey("ADM_PK")]
public class AdministratorModel : IDbModel
{
	[Key]
	public Guid ADM_PK { get; set; }

	[ForeignKey("AMD_USR_ID")]
	public UserModel ADM_User { get; set; }
	public Guid ADM_USR_ID { get; set; }
}