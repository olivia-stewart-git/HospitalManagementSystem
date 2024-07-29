using HMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

	string DbPath => Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName, "Database");

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		options
			.UseSqlite($"DataSource={DbPath}");
	}
	//.UseSqlServer($"Server=localhost;Initial Catalog=HospitalDb;Integrated Security=SSPI;TrustServerCertificate=True");
}