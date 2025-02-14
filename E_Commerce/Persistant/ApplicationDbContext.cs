
using System.Reflection;
using System.Security.Claims;

namespace E_Commerce.Persistant
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options , IHttpContextAccessor httpContextAccessor) : 
        IdentityDbContext<ApplicationUser>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();
                entity.HasIndex(x => x.Name).IsUnique();
            });
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entryEntites = ChangeTracker.Entries<AuditableEntites>();
            foreach (var item in entryEntites) 
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (item.State == EntityState.Added)
                {
                    item.Property(x => x.CreatedById).CurrentValue = currentUserId!;
                    item.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
                }
                else if (item.State == EntityState.Modified)
                {
                    item.Property(x => x.UpdatedById).CurrentValue = currentUserId!;
                    item.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
