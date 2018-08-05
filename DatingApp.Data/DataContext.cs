using DatingApp.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        
        // Entities
        public DbSet<Value> Values { get; set; }
    }
}