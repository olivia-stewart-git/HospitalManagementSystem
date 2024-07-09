using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Data.Models
{
    [PrimaryKey("USR_PK")]
    public class UserModel
    {
        [Key]
        public Guid USR_PK { get; set; }

        [Required]
        public int USR_ID { get; set; }

        [Required]
        public string USR_Password { get; set; }
    }

    [PrimaryKey("PAT_PK")]
    public class PatientModel
    {
	    [Key]
	    public Guid PAT_PK { get; set; }

        [ForeignKey("USR_ID")]
        public UserModel PAT_User { get; set; }
    }
}
