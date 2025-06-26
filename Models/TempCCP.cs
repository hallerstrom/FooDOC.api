using System;

namespace FooDOC.api.Models;

public class TempCCP
{
    public int Id { get; set; }
    public string Product { get; set; } = string.Empty;
    public double Temp { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
}
