using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class Payment
{
    public int Id { get; set; }

    public DateOnly PaymentDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal Amount { get; set; }

    public int ParentId { get; set; }

    public virtual Parent Parent { get; set; } = null!;
}
