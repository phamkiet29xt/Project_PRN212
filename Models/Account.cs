using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
