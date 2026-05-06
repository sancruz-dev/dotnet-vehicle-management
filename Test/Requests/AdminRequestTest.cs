using System.Net;
using System.Text;
using System.Text.Json;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.ModelViews;
using Test.Helpers;

namespace Test;

[TestClass]
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
    public async Task TestaGetSetPropriedades() 
    { 
        // Arrange
        var loginDTO = new LoginDTO()
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

        Assert.IsNotNull(adminLogado?.Email ?? "");
        Assert.IsNotNull(adminLogado?.Perfil ?? "");
        Assert.IsNotNull(adminLogado?.Token ?? "");
    }
}
