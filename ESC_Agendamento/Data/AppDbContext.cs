using Microsoft.EntityFrameworkCore;
using ESC_Agendamento.Models;

namespace ESC_Agendamento.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
