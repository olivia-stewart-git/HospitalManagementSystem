using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
}
