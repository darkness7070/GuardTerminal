using System;
using System.Collections.Generic;

namespace GuardTerminal.Models;

public partial class Subdivision
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Worker> Workers { get; } = new List<Worker>();
}
