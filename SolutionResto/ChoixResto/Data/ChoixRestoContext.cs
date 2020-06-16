using ChoixResto.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoixResto.Data
{
    public class ChoixRestoContext : DbContext
    {
        public ChoixRestoContext(DbContextOptions<ChoixRestoContext> options)
            : base(options)
        {
        }
        
        public DbSet<Sondage> Sondages { get; set; }
        public DbSet<Resto> Restos { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}