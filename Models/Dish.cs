using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class Dish
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Decription { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Staff CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();
}
