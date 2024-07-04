using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Models
{
    [PrimaryKey("RM_Id")]
    public class RoleModel
    {
	    [Key]
	    public Guid RM_Id;

        [Required]
        public string RM_Name;
    }
}
