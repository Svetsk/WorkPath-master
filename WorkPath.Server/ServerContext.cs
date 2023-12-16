using Microsoft.EntityFrameworkCore;
using WorkPath.Server.Entities;

namespace WorkPath.Server;

public class ServerContext: DbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<EducationEntity> Educations => Set<EducationEntity>();
    public DbSet<CompanyEntity> Companies => Set<CompanyEntity>();
    public DbSet<JobEntity> Jobs => Set<JobEntity>();
    
    public ServerContext()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public ServerContext(DbContextOptions<ServerContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}