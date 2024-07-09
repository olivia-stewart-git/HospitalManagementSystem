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

        public string USR_FirstName { get; set; }
        public string USR_LastName { get; set; }
        public string USR_PhoneNumber { get; set; }
        public string USR_Email { get; set; }

        public string USR_Address_State { get; set; }
        public string USR_Address_Postcode { get; set; }
        public string USR_Address_Line1 { get; set; }
        public string USR_Address_Line2 { get; set; }
    }
}
