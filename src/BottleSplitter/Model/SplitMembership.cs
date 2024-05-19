using System.Diagnostics.CodeAnalysis;

namespace BottleSplitter.Model;

public class SplitMembership : Auditable
{
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public SplitterUser User { get; set; } = default!;

    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public BottleSplit Split { get; set; } = default!;
    public int? Amount { get; set; }
    public bool? Paid { get; set; }
    public bool? Shipped { get; set; }
}
