using System;
using System.Collections.Generic;

namespace GuardTerminal.Models;

public partial class Visitor
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Organization { get; set; } = null!;

    public string Notes { get; set; } = null!;

    public DateTime DateBirth { get; set; }

    public string Series { get; set; } = null!;

    public string Numbers { get; set; } = null!;

    public bool IsBlacklist { get; set; }

    public byte[] Image { get; set; } = null!;

    public virtual ICollection<AppVi> AppVis { get; } = new List<AppVi>();
}
