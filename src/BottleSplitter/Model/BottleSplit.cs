using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BottleSplitter.Model;

public class BottleSplit : Auditable
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public SplitterUser Owner { get; set; } = default!;

    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public string Squid { get; set; } = default!;

    public SplitSettings Settings { get; set; } = new ();
}

public class SplitSettings
{
    public int? TotalAvailable { get; set; } = 700;//milliliters

    public List<int> Sizes { get; set; } = [50, 100];

    public string? Description { get; set; }
    public string? DetailsUrl { get; set; }
    public string? ImageUrl { get; set; }
}
