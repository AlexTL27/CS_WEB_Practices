using Microsoft.EntityFrameworkCore;
using WebChat.Modelos;

namespace WebChat.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Mensaje> Mensajes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Mensaje>(m =>
        {
            m.HasKey(e => e.Id);
            m.Property(e => e.Id).UseIdentityColumn();
        });
    } 
}