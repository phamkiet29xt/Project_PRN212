using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class Registration
{
    public int Id { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public string Status { get; set; } = null!;

    public int StudentId { get; set; }

    public int MenuId { get; set; }

    public virtual Menu Menu { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
