using System;
using System.Diagnostics.CodeAnalysis;

namespace BottleSplitter.Model;

public class BottleSplitMembers
{
    public Guid Id { get; set; } = Guid.Empty;
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public BottleSplit Split  { get; set; }= default!;
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public SplitterUser Member  { get; set; }= default!;

    public int? Amount { get; set; } //milliliters
}
