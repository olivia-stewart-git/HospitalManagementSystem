﻿// <auto-generated />
using System;
using HMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HMS.Data.Migrations
{
    [DbContext(typeof(HMSDbContext))]
    partial class HMSDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HMS.Data.Models.AdministratorModel", b =>
                {
                    b.Property<Guid>("ADM_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ADM_USR_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AMD_USR_ID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ADM_PK");

                    b.HasIndex("AMD_USR_ID");

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("HMS.Data.Models.AppointmentModel", b =>
                {
                    b.Property<Guid>("APT_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("APT_AppointmentTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("APT_DCT_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("APT_Description")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<Guid>("APT_PAT_ID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("APT_PK");

                    b.HasIndex("APT_DCT_ID");

                    b.HasIndex("APT_PAT_ID");

                    b.ToTable("AppointmentModels");
                });

            modelBuilder.Entity("HMS.Data.Models.DoctorModel", b =>
                {
                    b.Property<Guid>("DCT_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DCT_USR_ID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DCT_PK");

                    b.HasIndex("DCT_USR_ID");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("HMS.Data.Models.PatientModel", b =>
                {
                    b.Property<Guid>("PAT_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DCT_PK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PAT_USR_ID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PAT_PK");

                    b.HasIndex("DCT_PK");

                    b.HasIndex("PAT_USR_ID");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("HMS.Data.Models.STMDataModel", b =>
                {
                    b.Property<Guid>("STM_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("STM_HasSeeded")
                        .HasColumnType("bit");

                    b.HasKey("STM_PK");

                    b.ToTable("StmData");
                });

            modelBuilder.Entity("HMS.Data.Models.UserModel", b =>
                {
                    b.Property<Guid>("USR_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("USR_Address_Line1")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("USR_Address_Line2")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("USR_Address_Postcode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("USR_Address_State")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("USR_Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("USR_FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("USR_ID")
                        .HasColumnType("int");

                    b.Property<string>("USR_LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("USR_Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("USR_PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("USR_PK");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HMS.Data.Models.AdministratorModel", b =>
                {
                    b.HasOne("HMS.Data.Models.UserModel", "ADM_User")
                        .WithMany()
                        .HasForeignKey("AMD_USR_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ADM_User");
                });

            modelBuilder.Entity("HMS.Data.Models.AppointmentModel", b =>
                {
                    b.HasOne("HMS.Data.Models.DoctorModel", "APT_Doctor")
                        .WithMany("DCT_Appointments")
                        .HasForeignKey("APT_DCT_ID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("HMS.Data.Models.PatientModel", "APT_Patient")
                        .WithMany("PAT_Appointments")
                        .HasForeignKey("APT_PAT_ID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("APT_Doctor");

                    b.Navigation("APT_Patient");
                });

            modelBuilder.Entity("HMS.Data.Models.DoctorModel", b =>
                {
                    b.HasOne("HMS.Data.Models.UserModel", "DCT_User")
                        .WithMany()
                        .HasForeignKey("DCT_USR_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DCT_User");
                });

            modelBuilder.Entity("HMS.Data.Models.PatientModel", b =>
                {
                    b.HasOne("HMS.Data.Models.DoctorModel", "PAT_Doctor")
                        .WithMany("DCT_Patients")
                        .HasForeignKey("DCT_PK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("HMS.Data.Models.UserModel", "PAT_User")
                        .WithMany()
                        .HasForeignKey("PAT_USR_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PAT_Doctor");

                    b.Navigation("PAT_User");
                });

            modelBuilder.Entity("HMS.Data.Models.DoctorModel", b =>
                {
                    b.Navigation("DCT_Appointments");

                    b.Navigation("DCT_Patients");
                });

            modelBuilder.Entity("HMS.Data.Models.PatientModel", b =>
                {
                    b.Navigation("PAT_Appointments");
                });
#pragma warning restore 612, 618
        }
    }
}
