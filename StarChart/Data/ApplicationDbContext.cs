using Microsoft.EntityFrameworkCore;
using StarChart.Models;

namespace StarChart.Data
{
    public class ApplicationDbContext : DbContext
    {
        #region DbSets
        public DbSet<CelestialObject> CelestialObjects { get; set; }
        #endregion

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
