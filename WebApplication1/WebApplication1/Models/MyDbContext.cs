using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<UserTable> UserTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PC186;Database= LibraryManagement;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Book__C223F3B4BCFF0988");

            entity.ToTable("Book");

            entity.Property(e => e.BookId).HasColumnName("Book_Id");
            entity.Property(e => e.BookAuthor)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Book_Author");
            entity.Property(e => e.BookCategory)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Book_Category");
            entity.Property(e => e.BookStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Book_Status");
            entity.Property(e => e.BookTitle)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Book_Title");
            entity.Property(e => e.BorrowDate).HasColumnName("Borrow_Date");
            entity.Property(e => e.DueDate).HasColumnName("Due_Date");
            entity.Property(e => e.IsBorrowed).HasDefaultValue(false);
            entity.Property(e => e.ReturnDate).HasColumnName("Return_Date");

            entity.HasOne(d => d.User).WithMany(p => p.Books)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Book_User");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__CD399318A89DF201");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("Feedback_Id");
            entity.Property(e => e.FeedbackType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Feedback_Type");
            entity.Property(e => e.Message)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.SubmittedDate).HasColumnName("Submitted_Date");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__UserId__59FA5E80");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__19EE6A138F2F8355");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("Room_Id");
            entity.Property(e => e.Capacity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EndTime).HasColumnName("End_Time");
            entity.Property(e => e.IsReserved).HasDefaultValue(false);
            entity.Property(e => e.ReservationDate).HasColumnName("Reservation_Date");
            entity.Property(e => e.ReservationDuration).HasColumnName("Reservation_Duration");
            entity.Property(e => e.ReservationStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Reservation_Status");
            entity.Property(e => e.RoomName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Room_Name");
            entity.Property(e => e.RoomStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Room_Status");
            entity.Property(e => e.StartTime).HasColumnName("Start_Time");

            entity.HasOne(d => d.User).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room__UserId__5629CD9C");
        });

        modelBuilder.Entity<UserTable>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserTabl__1788CC4CE51E0511");

            entity.ToTable("UserTable");

            entity.HasIndex(e => e.Email, "UQ__UserTabl__A9D105341CD7AAAF").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
            entity.Property(e => e.OnBlackList)
                .HasDefaultValue(false)
                .HasColumnName("On_BlackList");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.ProfilePicture)
                .IsUnicode(false)
                .HasColumnName("Profile_Picture");
            entity.Property(e => e.Role)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.StudStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Stud_Status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
