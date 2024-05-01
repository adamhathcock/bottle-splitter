using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BottleSplitter.Model;

internal class SplitterUser : IdentityUser
{
    public IEnumerable<IdentityRole>? Roles { get; set; }
}
