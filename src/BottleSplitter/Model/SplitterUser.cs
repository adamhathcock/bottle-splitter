using System;

namespace BottleSplitter.Model;

public class SplitterUser
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Email { get; set; } = string.Empty;
    public UserSource Source { get; set; } = UserSource.Unknown;

    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? Postcode { get; set; }
}
