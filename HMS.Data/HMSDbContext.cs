using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data
{
    internal class HMSDbContext : DbContext 
    {
        public string DbPath { get; }
        public DbSet<RoleModel> Roles { get; set; }


        public HMSDbContext()
        {
	        var folder = Environment.SpecialFolder.LocalApplicationData;
	        var path = Environment.GetFolderPath(folder);
	        DbPath = System.IO.Path.Join(path, "HMS.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
	        => options.UseSqlite($"Data Source={DbPath};");
    }
}
