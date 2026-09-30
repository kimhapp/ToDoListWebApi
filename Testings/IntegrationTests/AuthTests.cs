using System.Net;
using System.Net.Http.Json;
using ToDoListWebApi.Dtos;

namespace Testings.IntegrationTests
{
    public class AuthTests(ToDoListWebApiFactory factory) : IClassFixture<ToDoListWebApiFactory>
    {        
        readonly HttpClient client = factory.CreateClient();

        readonly string registerRoute = "/api/auth/register";
        readonly string loginRoute = "/api/auth/login";
        readonly string name = "Test";
        readonly string password = "Test1234";

        static string NewEmail() => $"{Guid.NewGuid()}@gmail.com";

        #region POST /api/auth/register

        [Fact]
        public async Task Register_WithNewEmail_ReturnsToken()
        {
            // Arrange
            string email = NewEmail();

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(registerRoute, new
            {
                name = name,
                email = email,
                password = password
            });

            // Assert
            response.EnsureSuccessStatusCode();

            TokenResponseDto? body = await response.Content.ReadFromJsonAsync<TokenResponseDto>();

            Assert.NotNull(body);
            Assert.NotEmpty(body!.Token);
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_ReturnsConflict()
        {
            // Arrange
            string email = NewEmail();

            // Act
            await client.PostAsJsonAsync(registerRoute, new
            {
                name = name,
                email = email,
                password = password
            });

            HttpResponseMessage response = await client.PostAsJsonAsync(registerRoute, new
            {
                name = name,
                email = email,
                password = password
            });

            // Assert
            string body = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.Contains("Email already exists", body);
        }

        #endregion

        #region POST /api/auith/login

        [Fact]
        public async Task Login_WithCorrectInformation_ReturnsToken()
        {
            // Arrange
            string email = NewEmail();

            // Act
            await client.PostAsJsonAsync(registerRoute, new
            {
                name = name,
                email = email,
                password = password
            });

            HttpResponseMessage response = await client.PostAsJsonAsync(loginRoute, new
            {
                email = email,
                password = password
            });

            // Assert
            response.EnsureSuccessStatusCode();

            TokenResponseDto? body = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
            
            Assert.NotNull(body);
            Assert.NotEmpty(body!.Token);
        }

        [Fact]
        public async Task Login_WithWrongInformation_ReturnsUnauthorized()
        {
            // Arrange
            string email = NewEmail();

            // Act
            await client.PostAsJsonAsync(registerRoute, new
            {
                name = name,
                email = email,
                password = password
            });

            HttpResponseMessage response = await client.PostAsJsonAsync(loginRoute, new
            {
                email = "Wrong Email@gmail.com",
                password = "WrongPassword"
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion
    }
}