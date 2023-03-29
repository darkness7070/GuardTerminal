using System;
using System.Collections.Generic;

namespace GuardTerminal.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; } = new List<Application>();
}
