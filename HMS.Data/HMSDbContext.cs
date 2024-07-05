﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data
{
    public class HMSDbContext : DbContext 
    {
        public DbSet<UserModel> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
	        => options.UseSqlServer($"Server=localhost;Initial Catalog=HospitalDb;Integrated Security=SSPI;TrustServerCertificate=True");
    }
}
