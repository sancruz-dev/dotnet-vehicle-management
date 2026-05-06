using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using minimal_api.Domain.Entities;
using BCrypt.Net;

namespace minimal_api.infrastructure.DB
{
    public class MinimalApiContext : DbContext
    {
        private readonly IConfiguration _configuracaoAppSettings;

        public MinimalApiContext(IConfiguration configuracaoAppSettings)
        {
            _configuracaoAppSettings = configuracaoAppSettings;
        }

        public DbSet<Admin> Admins { get; set; } = default!;
        public DbSet<Veiculo> Veiculos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Email = "admin@teste.com",
                    Senha = "$2a$11$PCSAjVjLGcFCt3fS1O71Tez4/26cyuER56xY0DQ7sNUd.WklLQgeC",
                    Perfil = "Adm"
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var stringConexao = _configuracaoAppSettings.GetConnectionString("Postgresql");

                if (!string.IsNullOrEmpty(stringConexao))
                {
                    optionsBuilder.UseNpgsql(stringConexao);
                }
            }
        }
    }
}