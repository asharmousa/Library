using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string BookTitle { get; set; } = null!;

    public string BookAuthor { get; set; } = null!;

    public string? BookCategory { get; set; }

    public string BookStatus { get; set; } = null!;

    public int? UserId { get; set; }

    public DateOnly? BorrowDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public bool? IsBorrowed { get; set; }

    public virtual UserTable? User { get; set; }
}
