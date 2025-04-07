using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class Menu
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public string MealTime { get; set; } = null!;

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
