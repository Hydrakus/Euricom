using System.Data.Entity;

namespace Euricom.Cruise2018.Demo.Query
{
    public class QueryContext : DbContext
    {
        public QueryContext()
           : base("Cruise2018Demo")
        {
            Configuration.LazyLoadingEnabled = false;
            
            Database.SetInitializer(new QueryContextInitializer());

        }

        public virtual DbSet<PapierSettingPersoon.PapierSettingPersoon> PapierSettingsPersoon { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PapierSettingPersoon.PapierSettingPersoon>()
               .HasKey(psp => psp.PapierSettingPersoonId, cfg => cfg.IsClustered(false));

        }

    }
}
