using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Models;

[PrimaryKey("ADM_PK")]
public class AdministratorModel
{
	[Key]
	public Guid ADM_PK { get; set; }

	[ForeignKey("USR_ID")]
	public UserModel ADM_User { get; set; }
}