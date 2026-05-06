using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Domain.Entities;

namespace Test.Domain.Entities
{
    [TestClass]
    [TestCategory("Unit")]
    public class AdminTest
    {
        [TestMethod]
        public void TestaGetSetPropriedades()
        {
            // Arrange
            var adm = new Admin();

            // Act
            adm.Id = 1;
            adm.Email = "admin@teste.com";
            adm.Senha = "123456";
            adm.Perfil = "Adm";

            // Assert
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("admin@teste.com", adm.Email);
            Assert.AreEqual("123456", adm.Senha);
            Assert.AreEqual("Adm", adm.Perfil);
        }
    }
}