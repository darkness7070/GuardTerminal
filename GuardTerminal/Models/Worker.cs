using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GuardTerminal.Models;

public partial class Worker
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public int IdSubdivision { get; set; }
    [JsonIgnore]
    public int IdRole { get; set; }

    public string Code { get; set; } = null!;
    [JsonIgnore]

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual Subdivision IdSubdivisionNavigation { get; set; } = null!;
}
