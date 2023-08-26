using Genetics.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Genetics.Server.Dal
{
    public class AnimalContext : DbContext
    {
        public DbSet<Animal> Animal { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

        public AnimalContext(DbContextOptions<AnimalContext> options) : base(options)
        { 
        
        }
    }
}
