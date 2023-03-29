using System;
using System.Collections.Generic;

namespace GuardTerminal.Models;

public partial class AppVi
{
    public int Id { get; set; }

    public int IdApp { get; set; }

    public int IdVis { get; set; }

    public virtual Application IdAppNavigation { get; set; } = null!;

    public virtual Visitor IdVisNavigation { get; set; } = null!;
}
