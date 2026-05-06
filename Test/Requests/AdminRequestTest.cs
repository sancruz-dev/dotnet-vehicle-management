using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.ModelViews;
using Test.Helpers;

namespace Test.Requests
{
    [TestClass]
    [TestCategory("Unit")]
    public class AdminRequestTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Setup.ClassInit(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.ClassCleanup();
        }

        [TestMethod]
        public async Task LoginComCredenciaisValidas_DeveRetornarOkComToken()
        {
            // Arrange
            var loginDTO = new LoginDTO
            {
                Email = "mockAdm@teste.com",
                Senha = "123456"
            };
            var content = new StringContent(
                JsonSerializer.Serialize(loginDTO),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var response = await Setup.client.PostAsync("/Admins/login", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var adminLogado = JsonSerializer.Deserialize<AdminLogadoModelView>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(adminLogado);
            Assert.IsFalse(string.IsNullOrEmpty(adminLogado.Email));
            Assert.IsFalse(string.IsNullOrEmpty(adminLogado.Perfil));
            Assert.IsFalse(string.IsNullOrEmpty(adminLogado.Token));
        }

        [TestMethod]
        public async Task LoginComCredenciaisInvalidas_DeveRetornarUnauthorized()
        {
            // Arrange
            var loginDTO = new LoginDTO
            {
                Email = "naoexiste@teste.com",
                Senha = "senhaerrada"
            };
            var content = new StringContent(
                JsonSerializer.Serialize(loginDTO),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var response = await Setup.client.PostAsync("/Admins/login", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}