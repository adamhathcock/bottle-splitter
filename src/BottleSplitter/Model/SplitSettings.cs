using System.Collections.Generic;

namespace BottleSplitter.Model;

public class SplitSettings
{
    public int? TotalAvailable { get; set; } = 700; //milliliters

    public List<int> Sizes { get; set; } = [50, 100];

    public string? Description { get; set; }
    public string? DetailsUrl { get; set; }
    public string? ImageUrl { get; set; }
}
