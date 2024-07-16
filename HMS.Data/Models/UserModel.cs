using HMS.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Data.Models;

[PrimaryKey("USR_PK")]
public class UserModel : IDbModel
{
	[Key]
	public Guid USR_PK { get; set; }

	[Required]
	public int USR_ID { get; set; }

	[Required]
	[MaxLength(100)]
	public string USR_Password { get; set; }

	[MaxLength(50)]
    public string USR_FirstName { get; set; }

	[MaxLength(50)]
    public string USR_LastName { get; set; }

	[MaxLength(20)]
    public string USR_PhoneNumber { get; set; }

	[MaxLength(100)]
    public string USR_Email { get; set; }

	[MaxLength(100)]
    public string USR_Address_State { get; set; }

	[MaxLength(20)]
    public string USR_Address_Postcode { get; set; }

	[MaxLength(100)]
    public string USR_Address_Line1 { get; set; }

	[MaxLength(100)]
    public string USR_Address_Line2 { get; set; }

	[NotMapped]
	public string USR_FullName => USR_FirstName + " " + USR_LastName;
}