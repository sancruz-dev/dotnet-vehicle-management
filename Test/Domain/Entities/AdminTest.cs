using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Domain.Entities;

namespace Test.Domain.Entities
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void TestaGetSetPropriedades()
        {
            // Arrange
            var adm = new Admin();

            // Act (set)
            adm.Id = 1;
            adm.Email = "admin@teste.com";
            adm.Senha = "123456";
            adm.Perfil = "Adm";

            // Assert (get)
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("admin@teste.com", adm.Email);
            Assert.AreEqual("123456", adm.Senha);
            Assert.AreEqual("Adm", adm.Perfil);
        }
    }
}