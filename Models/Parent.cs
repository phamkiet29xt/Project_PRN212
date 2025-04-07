using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class Parent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Mobile { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
