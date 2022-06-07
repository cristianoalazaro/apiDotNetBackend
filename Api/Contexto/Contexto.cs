using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Context
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        public DbSet<Produto> Produto { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
