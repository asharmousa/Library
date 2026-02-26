using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int UserId { get; set; }

    public string? Message { get; set; }

    public string? FeedbackType { get; set; }

    public int? Rating { get; set; }

    public DateOnly? SubmittedDate { get; set; }

    public bool IsApproved { get; set; }

    public virtual UserTable User { get; set; } = null!;
}
