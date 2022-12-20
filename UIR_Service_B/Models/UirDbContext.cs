using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UIR_Service_B.Models;

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

    public virtual DbSet<InvitesCurrent> InvitesCurrents { get; set; }

    public virtual DbSet<InvitesEnded> InvitesEndeds { get; set; }

    public virtual DbSet<Pass> Passes { get; set; }

    public virtual DbSet<RatingScale> RatingScales { get; set; }

    public virtual DbSet<RecordCurrent> RecordCurrents { get; set; }

    public virtual DbSet<RecordEnded> RecordEndeds { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<UserTable> UserTables { get; set; }

    public virtual DbSet<WeekDay> WeekDays { get; set; }

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

        modelBuilder.Entity<InvitesCurrent>(entity =>
        {
            entity.HasKey(e => new { e.RecordId, e.UserUirId }).HasName("PK_Invites_Current");

            entity.ToTable("Invites_Current");

            entity.Property(e => e.RecordId)
                .ValueGeneratedNever()
                .HasColumnName("Record_ID");
            entity.Property(e => e.AdditionalInfo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Additional_Info");
            entity.Property(e => e.UserUirId).HasColumnName("UserUIR_ID");

            entity.HasOne(d => d.Record).WithMany(p => p.InvitesCurrent)
                .HasForeignKey(d => d.RecordId)
                .HasConstraintName("R_25");

            entity.HasOne(d => d.UserUir).WithMany(p => p.InvitesCurrents)
                .HasForeignKey(d => d.UserUirId)
                .HasConstraintName("R_26");
        });

        modelBuilder.Entity<InvitesEnded>(entity =>
        {
            entity.HasKey(e => new { e.RecordId, e.UserUirId }).HasName("PK_Invites_Ended");

            entity.ToTable("Invites_Ended");

            entity.Property(e => e.RecordId)
                .ValueGeneratedNever()
                .HasColumnName("Record_ID");
            entity.Property(e => e.AdditionalInfo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Additional_Info");
            entity.Property(e => e.UserUirId).HasColumnName("UserUIR_ID");

            entity.HasOne(d => d.Record).WithMany(p => p.InvitesEnded)
                .HasForeignKey(d => d.RecordId)
                .HasConstraintName("R_36");

            entity.HasOne(d => d.UserUir).WithMany(p => p.InvitesEndeds)
                .HasForeignKey(d => d.UserUirId)
                .HasConstraintName("R_37");
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

        modelBuilder.Entity<RatingScale>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Rating_S__BE48C82598A677E7");

            entity.ToTable("Rating_Scale");

            entity.Property(e => e.RatingId).HasColumnName("Rating_ID");
        });

        modelBuilder.Entity<RecordCurrent>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Record_C__603A0C60E81CDFD3");

            entity.ToTable("Record_Current");

            entity.Property(e => e.RecordId).HasColumnName("Record_ID");
            entity.Property(e => e.RoomId).HasColumnName("Room_ID");
            entity.Property(e => e.UserUirId).HasColumnName("UserUIR_ID");
            entity.Property(e => e.From1).HasColumnType("datetime");
            entity.Property(e => e.To1).HasColumnType("datetime");

            entity.HasOne(d => d.Room).WithMany(p => p.RecordCurrents)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("R_17");

            entity.HasOne(d => d.UserUir).WithMany(p => p.RecordCurrents)
                .HasForeignKey(d => d.UserUirId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("R_20");
        });

        modelBuilder.Entity<RecordEnded>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Record_E__603A0C60A1FFC6FD");

            entity.ToTable("Record_Ended");

            entity.Property(e => e.RecordId)
                .ValueGeneratedNever()
                .HasColumnName("Record_ID");
            entity.Property(e => e.RatingId).HasColumnName("Rating_ID");
            entity.Property(e => e.RoomId).HasColumnName("Room_ID");
            entity.Property(e => e.UserUirId).HasColumnName("UserUIR_ID");
            entity.Property(e => e.From1).HasColumnType("datetime");
            entity.Property(e => e.To1).HasColumnType("datetime");

            entity.HasOne(d => d.Rating).WithMany(p => p.RecordEndeds)
                .HasForeignKey(d => d.RatingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("R_33");

            entity.HasOne(d => d.Room).WithMany(p => p.RecordEndeds)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("R_35");

            entity.HasOne(d => d.UserUir).WithMany(p => p.RecordEndeds)
                .HasForeignKey(d => d.UserUirId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("R_34");
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
                .HasMaxLength(20)
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

        modelBuilder.Entity<WeekDay>(entity =>
        {
            entity.HasKey(e => e.WeekdayId).HasName("PK__Week_Day__D07E077FD8F8AE9B");

            entity.ToTable("Week_Day");

            entity.Property(e => e.WeekdayId)
                .ValueGeneratedNever()
                .HasColumnName("Weekday_ID");
            entity.Property(e => e.WeekDay1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Week_Day");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
