using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UIR_WebAPI_1.Models;

public partial class UirDbContext : DbContext
{
    public UirDbContext()
    {
    }

    public UirDbContext(DbContextOptions<UirDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<AppointmentCurrent> AppointmentCurrents { get; set; }

    public virtual DbSet<AppointmentEnded> AppointmentEndeds { get; set; }

    public virtual DbSet<Pass> Passes { get; set; }

    public virtual DbSet<PassGarmony> PassesGarmony { get; set; }

    public virtual DbSet<RatingScale> RatingScales { get; set; }

    public virtual DbSet<SheduleTable> SheduleTables { get; set; }

    public virtual DbSet<Specialist> Specialists { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<UserTable> UserTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Name=UIR_DB");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__Area__4256772EFD6BCDC8");

            entity.ToTable("Area");

            entity.Property(e => e.AreaId).HasColumnName("Area_ID");
            entity.Property(e => e.AreaLocation)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Area_Location");
            entity.Property(e => e.AreaName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Area_Name");
            entity.Property(e => e.From1).HasPrecision(0);
            entity.Property(e => e.To1).HasPrecision(0);
        });

        modelBuilder.Entity<AppointmentCurrent>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__FD01B503B90BC8C5");

            entity.ToTable("Appointment_Current");

            entity.Property(e => e.AppointmentId).HasColumnName("Appointment_ID");
            entity.Property(e => e.From1).HasColumnType("datetime");
            entity.Property(e => e.SpecialistId).HasColumnName("Specialist_ID");
            entity.Property(e => e.To1).HasColumnType("datetime");
            entity.Property(e => e.UserUirId).HasColumnName("UserUIR_ID");

            entity.HasOne(d => d.Specialist).WithMany(p => p.AppointmentCurrents)
                .HasForeignKey(d => d.SpecialistId)
                .HasConstraintName("R_7");

            entity.HasOne(d => d.UserUir).WithMany(p => p.AppointmentCurrents)
                .HasForeignKey(d => d.UserUirId)
                .HasConstraintName("R_9");
        });

        modelBuilder.Entity<AppointmentEnded>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__FD01B50304DC5374");

            entity.ToTable("Appointment_Ended");

            entity.Property(e => e.AppointmentId)
                .ValueGeneratedNever()
                .HasColumnName("Appointment_ID");
            entity.Property(e => e.From1).HasColumnType("datetime");
            entity.Property(e => e.RatingId).HasColumnName("Rating_ID");
            entity.Property(e => e.SpecialistId).HasColumnName("Specialist_ID");
            entity.Property(e => e.To1).HasColumnType("datetime");
            entity.Property(e => e.UserUirId).HasColumnName("UserUIR_ID");

            entity.HasOne(d => d.Rating).WithMany(p => p.AppointmentEndeds)
                .HasForeignKey(d => d.RatingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("R_32");

            entity.HasOne(d => d.Specialist).WithMany(p => p.AppointmentEndeds)
                .HasForeignKey(d => d.SpecialistId)
                .HasConstraintName("R_31");

            entity.HasOne(d => d.UserUir).WithMany(p => p.AppointmentEndeds)
                .HasForeignKey(d => d.UserUirId)
                .HasConstraintName("R_30");
        });

        modelBuilder.Entity<Pass>(entity =>
        {
            entity.HasKey(e => e.UserUirId).HasName("PK__Pass");

            entity.ToTable("Pass");

            entity.Property(e => e.UserUirId)
                .ValueGeneratedNever()
                .HasColumnName("UserUIR_ID");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("Password_Hash");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("User_login");

            entity.HasOne(d => d.UserUir).WithOne(p => p.Pass)
                .HasForeignKey<Pass>(d => d.UserUirId)
                .HasConstraintName("R_28");
        });

        modelBuilder.Entity<PassGarmony>(entity =>
        {
            entity.HasKey(e => e.SpecialistID).HasName("PK_Pass_Garmony");

            entity.ToTable("Pass_Garmony");

            entity.Property(e => e.SpecialistID)
                .ValueGeneratedNever()
                .HasColumnName("Specialist_ID");
            entity.Property(e => e.Password)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("Password");
            entity.Property(e => e.Login)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("Login");

            entity.HasOne(d => d.Specialist).WithOne(p => p.PassDarmonyNavigation)
                .HasForeignKey<PassGarmony>(d => d.SpecialistID)
                .HasConstraintName("FK_Pass_Garmony_Specialist");
        });

        modelBuilder.Entity<RatingScale>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Rating_S__BE48C82598A677E7");

            entity.ToTable("Rating_Scale");

            entity.Property(e => e.RatingId).HasColumnName("Rating_ID");
        });

        modelBuilder.Entity<SheduleTable>(entity =>
        {
            entity.HasKey(e => new { e.SpecialistId, e.WeekdayId }).HasName("PK_Shedule_Specialist_ID_Weekday_ID");

            entity.ToTable("SheduleTable");

            entity.Property(e => e.SpecialistId).HasColumnName("Specialist_ID");
            entity.Property(e => e.WeekdayId).HasColumnName("Weekday_ID");
            entity.Property(e => e.From1).HasPrecision(0);
            entity.Property(e => e.LunchEnd)
                .HasPrecision(0)
                .HasColumnName("Lunch_End");
            entity.Property(e => e.LunchStart)
                .HasPrecision(0)
                .HasColumnName("Lunch_Start");
            entity.Property(e => e.To1).HasPrecision(0);
            entity.Property(e => e.Price).HasColumnName("Price");
            entity.HasOne(d => d.Specialist).WithMany(p => p.SheduleTables)
                .HasForeignKey(d => d.SpecialistId)
                .HasConstraintName("R_3");
        });

        modelBuilder.Entity<Specialist>(entity =>
        {
            entity.HasKey(e => e.SpecialistId).HasName("PK__Speciali__5AA3872F5ACB0C00");

            entity.ToTable("Specialist");

            entity.Property(e => e.SpecialistId)
                .ValueGeneratedNever()
                .HasColumnName("Specialist_ID");
            entity.Property(e => e.AdditionalInfo)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("Additional_Info");
            entity.Property(e => e.Degree)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Experience)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Specialization)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.SpecialistNavigation).WithOne(p => p.Specialist)
                .HasForeignKey<Specialist>(d => d.SpecialistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("R_2");

            entity.HasMany(d => d.Rooms).WithMany(p => p.Specs)
                .UsingEntity<Dictionary<string, object>>(
                    "SpecRoom",
                    r => r.HasOne<Room>().WithMany()
                        .HasForeignKey("RoomId")
                        .HasConstraintName("Spec_Rooms_Room_ID"),
                    l => l.HasOne<Specialist>().WithMany()
                        .HasForeignKey("SpecId")
                        .HasConstraintName("Spec_Rooms_Spec_ID"),
                    j =>
                    {
                        j.HasKey("SpecId", "RoomId");
                        j.ToTable("Spec_Rooms");
                        j.IndexerProperty<int>("SpecId").HasColumnName("Spec_ID");
                        j.IndexerProperty<int>("RoomId").HasColumnName("Room_ID");
                    });
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__19EE6A737A370408");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("Room_ID");
            entity.Property(e => e.AdditionalInformation)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Additional_information");
            entity.Property(e => e.AreaId).HasColumnName("Area_ID");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Room_Number");

            entity.HasOne(d => d.Area).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("R_16");
        });

        modelBuilder.Entity<UserTable>(entity =>
        {
            entity.HasKey(e => e.UserUirId).HasName("PK__UserTabl__937F90ACC0A253CF");

            entity.ToTable("UserTable");

            entity.Property(e => e.UserUirId).HasColumnName("UserUIR_ID");
            entity.Property(e => e.EMail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("e_mail");
            entity.Property(e => e.FirstName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Middle_Name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Phone_Number");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
