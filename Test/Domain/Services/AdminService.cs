using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Services;
using minimal_api.infrastructure.DB;

namespace Test.Domain.Services
{
    [TestClass]
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
            // Arrange
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE admins");

            var adm = new Admin();
            adm.Email = "MsTeste@teste.com";
            adm.Senha = "teste123";
            adm.Perfil = "Adm";

            var administradorServico = new AdminService(context);

            // Act
            administradorServico.Incluir(adm);

            // Assert
            Assert.AreEqual(2, administradorServico.Todos(1).Count());
        }

        [TestMethod]
        public void TestandoBuscaPorId()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE admins");

            var adm = new Admin();
            adm.Email = "MsTestBuscaPorId@teste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var adminService = new AdminService(context);

            // Act
            adminService.Incluir(adm);
            var admDoBanco = adminService.BuscaPorId(adm.Id);

            // Assert
            Assert.AreEqual(1, admDoBanco?.Id);
        }
    }
}
