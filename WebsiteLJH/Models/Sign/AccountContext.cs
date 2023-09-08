using Microsoft.EntityFrameworkCore;

namespace WebsiteLJH;

public class AccountContext : DbContext
{
    public AccountContext(DbContextOptions<AccountContext> options) : base(options)
    {

    }

    public DbSet<SignBase> SignBases { get; set; }
}
