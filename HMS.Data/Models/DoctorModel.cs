﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Models;

[PrimaryKey("DCT_PK")]
public class DoctorModel
{
	[Key]
	public Guid DCT_PK { get; set; }

	[ForeignKey("USR_ID")]
	public UserModel DCT_User { get; set; }
}