using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Services;
using minimal_api.infrastructure.DB;

namespace Test.Domain.Services
{
    [TestClass]
    [TestCategory("Integration")]
    public class AdminServiceTest
    {
        private MinimalApiContext CriarContextoDeTeste()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

            var builder = new ConfigurationBuilder()
                .SetBasePath(path ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            return new MinimalApiContext(configuration);
        }

        [TestMethod]
        public void TestandoSalvarAdministrador()
        {
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE \"Admins\" RESTART IDENTITY CASCADE");

            var adm = new Admin
            {
                Email = "MsTeste@teste.com",
                Senha = "teste123",
                Perfil = "Adm"
            };

            var administradorServico = new AdminService(context);
            administradorServico.Incluir(adm);

            // Seed insere 1 admin, mais o que acabamos de salvar = 2
            Assert.AreEqual(2, administradorServico.Todos(1).Count());
        }

        [TestMethod]
        public void TestandoBuscaPorId()
        {
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE \"Admins\" RESTART IDENTITY CASCADE");

            var adm = new Admin
            {
                Email = "MsTestBuscaPorId@teste.com",
                Senha = "teste",
                Perfil = "Adm"
            };

            var adminService = new AdminService(context);
            adminService.Incluir(adm);

            var admDoBanco = adminService.BuscaPorId(adm.Id);
            Assert.IsNotNull(admDoBanco);
            Assert.AreEqual(adm.Id, admDoBanco.Id);
        }
    }
}