using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HMS.Data.Models
{
    [PrimaryKey("USR_PK")]
    public class UserModel
    {
        [Key]
        public Guid USR_PK { get; set; }
    }
}
