using HMS.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Data.Models;

[PrimaryKey("USR_PK")]
public class UserModel : IDbModel, IUser
{
	[Key] public Guid USR_PK { get; set; } = Guid.NewGuid();

	[Required]
	public int USR_ID { get; set; }

	[Required]
	[MaxLength(100)]
	public string USR_Password { get; set; } = string.Empty;

    [MaxLength(50)]
    public string USR_FirstName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string USR_LastName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string USR_PhoneNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string USR_Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string USR_Address_State { get; set; } = string.Empty;

    [MaxLength(20)]
    public string USR_Address_Postcode { get; set; } = string.Empty;

	[MaxLength(100)]
    public string USR_Address_Line1 { get; set; } = string.Empty;

    [MaxLength(100)]
    public string USR_Address_Line2 { get; set; } = string.Empty;

    [NotMapped]
	public string USR_FullName => USR_FirstName + " " + USR_LastName;

	[NotMapped]
    public UserModel User { get => this; set => _ = value; }
}