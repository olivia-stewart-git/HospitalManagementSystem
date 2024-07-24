using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Models;

/// <summary>
/// Stores persisting app data. In this case, whether we have seeded or not
/// </summary>
[PrimaryKey("STM_PK")]
public class STMDataModel : IDbModel
{
    [Key]
    public Guid STM_PK { get; set; }

    [Required]
    public bool STM_HasSeeded { get; set; }
}