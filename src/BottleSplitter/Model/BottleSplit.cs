using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BottleSplitter.Model;

public class BottleSplit : Auditable
{
    [Required]
    public string Name { get; set; }= string.Empty;

    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public SplitterUser Owner { get; set; } = default!;

    public string? Description { get; set; }
    public string? DetailsUrl { get; set; }
    public string? ImageUrl { get; set; }


    public int? TotalAvailable { get; set; } //milliliters
}
