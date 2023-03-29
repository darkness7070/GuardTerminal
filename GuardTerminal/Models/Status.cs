using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GuardTerminal.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
