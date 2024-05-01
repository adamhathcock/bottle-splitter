using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BottleSplitter.Model;

internal class SplitterDbContext(DbContextOptions<SplitterDbContext> options)
    : IdentityDbContext<SplitterUser>(options);
