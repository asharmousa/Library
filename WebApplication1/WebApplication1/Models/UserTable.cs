using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class UserTable
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Role { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? ProfilePicture { get; set; }

    public string StudStatus { get; set; } = null!;

    public bool? OnBlackList { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
