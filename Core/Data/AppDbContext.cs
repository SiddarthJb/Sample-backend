using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using Z1.Auth.Models;
using Z1.Chat.Models;
using Z1.Match.Models;
using Z1.Profiles.Models;

namespace Z1.Core.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private IHttpContextAccessor httpContextAccessor;

        public AppDbContext(DbContextOptions dbContextOptions, IHttpContextAccessor httpContextAccessor) :
            base(dbContextOptions)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public DbSet<PendingUser> PendingUsers { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Interests> Interests { get; set; }
        //Filters and Match Queue
        public DbSet<Filter> Filters { get; set; }
        public DbSet<Languages> Languages { get; set; }
        public DbSet<MatchQueue> MatchQueue { get; set; }
        public DbSet<ShowMeFilter> ShowMeFilters { get; set; }
        public DbSet<RelationshipTypeFilter> RelationshipTypeFilters { get; set; }
        public DbSet<SmokeFilter> SmokeFilters { get; set; }
        public DbSet<AlcoholFilter> AlcoholFilters { get; set; }
        public DbSet<MaritalStatusFilter> MaritalStatusFilter { get; set; }
        public DbSet<KidsFilter> KidsFilters { get; set; }
        public DbSet<ReligionFilter> ReligionFilters { get; set; }
        public DbSet<EducationFilter> EducationFilters { get; set; }
        public DbSet<ZodiacFilter> ZodiacFilters { get; set; }
        public DbSet<ProfessionFilter> ProfessionFilters { get; set; }
        public DbSet<LanguagesFilter> LanguageFilters { get; set; }
        public DbSet<InterestsFilter> InterestFilters { get; set; }
        public DbSet<Matches> Matches { get; set; }

        //Chat
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>()
            //    .HasOne(e => e.Profile)
            //    .WithOne(e => e.User)
            //    .HasForeignKey<Profile>(e => e.UserId)
            //    .IsRequired();

            modelBuilder.Entity<Filter>()
                .HasOne(fp => fp.User)
                .WithMany()
                .HasForeignKey(fp => fp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchQueue>()
                .Property(s => s.Location)
                .HasColumnType("geography");

            //modelBuilder.Entity<MatchQueue>()
            //    .HasOne(fp => fp.User)
            //    .WithMany()
            //    .HasForeignKey(fp => fp.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            // Get all the entities that inherit from AuditableEntity
            // and have a state of Added or Modified
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            // For each entity we will set the Audit properties
            foreach (var entityEntry in entries)
            {
                // If the entity state is Added let's set
                // the CreatedAt and CreatedBy properties
                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                    ((AuditableEntity)entityEntry.Entity).CreatedBy = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
                }
                else
                {
                    // If the state is Modified then we don't want
                    // to modify the CreatedAt and CreatedBy properties
                    // so we set their state as IsModified to false
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }

                // In any case we always want to set the properties
                // ModifiedAt and ModifiedBy
                ((AuditableEntity)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;
                ((AuditableEntity)entityEntry.Entity).ModifiedBy = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
            }

            // After we set all the needed properties
            // we call the base implementation of SaveChanges
            // to actually save our entities in the database
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Get all the entities that inherit from AuditableEntity
            // and have a state of Added or Modified
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            // For each entity we will set the Audit properties
            foreach (var entityEntry in entries)
            {
                // If the entity state is Added let's set
                // the CreatedAt and CreatedBy properties
                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                    ((AuditableEntity)entityEntry.Entity).CreatedBy = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
                }
                else
                {
                    // If the state is Modified then we don't want
                    // to modify the CreatedAt and CreatedBy properties
                    // so we set their state as IsModified to false
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }

                // In any case we always want to set the properties
                // ModifiedAt and ModifiedBy
                ((AuditableEntity)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;
                ((AuditableEntity)entityEntry.Entity).ModifiedBy = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
            }

            // After we set all the needed properties
            // we call the base implementation of SaveChangesAsync
            // to actually save our entities in the database
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
