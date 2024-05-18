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

    public SplitSettings Settings { get; set; } = new();

    public List<SplitterUser> Members { get; set; } = [];
}
