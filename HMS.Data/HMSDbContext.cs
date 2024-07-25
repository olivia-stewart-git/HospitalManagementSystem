using HMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HMS.Data;

/// <summary>
/// Db Context implementation with simple connection string.
/// Not much happening here
/// </summary>
public class HMSDbContext : DbContext 
{
	public DbSet<STMDataModel> StmData { get; set; }
	public DbSet<UserModel> Users { get; set; }
	public DbSet<DoctorModel> Doctors { get; set; }
	public DbSet<PatientModel> Patients { get; set; }
	public DbSet<AdministratorModel> Administrators { get; set; }
	public DbSet<AppointmentModel> AppointmentModels { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options
			.UseSqlServer($"Server=localhost;Initial Catalog=HospitalDb;Integrated Security=SSPI;TrustServerCertificate=True");
}