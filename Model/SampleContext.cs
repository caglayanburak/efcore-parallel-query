using Microsoft.EntityFrameworkCore;

namespace EfCoreParallelSample.Model
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             optionsBuilder.UseSqlServer(@"Server=127.0.0.1,1433;Database=EfCoreSample;User Id=sa;password=Admin123;");
        }

        public DbSet<InventoryItem> InventoryItems { get; set; }
    }
}