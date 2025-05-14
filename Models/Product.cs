using System;

namespace FooDOC.api.Models;

public class Product
{
    public int Id { get; set; }

    public required string Name { get; set; } = string.Empty;

}
