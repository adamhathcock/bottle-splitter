using System;

namespace BottleSplitter.Model;

public class SplitterUser
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Email { get; set; } = string.Empty;
    public UserSource Source { get; set; } = UserSource.Unknown;
}

public enum UserSource
{
    Unknown = 0,
    Github = 1,
    Google = 2
}
