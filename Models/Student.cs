using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Birthdate { get; set; }

    public bool Gender { get; set; }
    public string gender => Gender ? "Male" : "Female";

    public int ClassId { get; set; }

    public int? ParentId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Parent? Parent { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
