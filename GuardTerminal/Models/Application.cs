using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GuardTerminal.Models;

public partial class Application
{
    public int Id { get; set; }

    public DateTime ValidatyFrom { get; set; }

    public DateTime ValidatyTo { get; set; }
    public string ValidatyStr
    {
        get
        {
            return $"с {ValidatyFrom.ToString("d")} до {ValidatyTo.ToString("d")}";
        }
        set { }
    }
    [JsonIgnore]
    public int IdPurpose { get; set; }
    [JsonIgnore]

    public int IdWorker { get; set; }

    public byte[]? Passport { get; set; }

    public DateTime? ArrivalTime { get; set; }
    public string ArrivalTimeStr 
    {
        get
        {
            return ArrivalTime.Value.ToString("d");
        }
        set { } 
    }

    public DateTime? LeavingTime { get; set; }

    public bool IsSingle { get; set; }
    public string IsSingleStr {
        get
        {
            return IsSingle ? "Одиночная" : "Групповая";
        }
        set { } 
    }

    public virtual ICollection<AppVi> AppVis { get; } = new List<AppVi>();

    public virtual Purpose IdPurposeNavigation { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual Worker IdWorkerNavigation { get; set; } = null!;
}
