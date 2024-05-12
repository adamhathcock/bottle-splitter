using System;

namespace BottleSplitter.Model;

public abstract class Auditable
{
    public Guid Id { get; set; } = Guid.Empty;
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateUpdated { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}
