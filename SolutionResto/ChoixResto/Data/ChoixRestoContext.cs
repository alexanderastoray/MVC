﻿using ChoixResto.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChoixResto.Data
{
    public class ChoixRestoContext : IdentityDbContext
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