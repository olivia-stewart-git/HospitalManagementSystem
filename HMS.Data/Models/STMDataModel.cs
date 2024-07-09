using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Models;

[PrimaryKey("STM_PK")]
public class STMDataModel
{
    [Key]
    public Guid STM_PK { get; set; }

    [Required]
    public bool STM_HasSeeded { get; set; }
}