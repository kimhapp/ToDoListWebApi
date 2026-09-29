using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using ToDoListWebApi.Dtos;

namespace Testings.IntegrationTests
{
    public class ToDoTests(ToDoListWebApiFactory factory) : IClassFixture<ToDoListWebApiFactory>
    {
        readonly string toDoRoute = "/api/todo/";

        async Task<HttpClient> RegisteredUser()
        {
            // Needs to create client per user
            // Otherwise if there are 2 users used in a method
            // The two users will share the same client instance
            // And cause the client to only fetch the latest registered user's data
            HttpClient client = factory.CreateClient();

            string route = "/api/auth/register";
            string name = "Test";
            string email = $"{Guid.NewGuid()}@gmail.com";
            string password = "Test1234";

            HttpResponseMessage response = await client.PostAsJsonAsync(route, new
            {
                name = name,
                email = email,
                password = password
            });

            TokenResponseDto? body = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", body!.Token);

            return client;
        }

        #region GET /api/todo

        [Fact]
        public async Task GetAllToDos_ByUserId_ReturnsList()
        {
            // Arrange
            HttpClient user = await RegisteredUser();

            // Act
            HttpResponseMessage response = await user.GetAsync(toDoRoute);

            // Assert
            response.EnsureSuccessStatusCode();

            List<ToDoDto>? toDoDtos = await response.Content.ReadFromJsonAsync<List<ToDoDto>>();
            Assert.NotNull(toDoDtos);
            Assert.Empty(toDoDtos);
        }

        [Fact]
        public async Task GetAllToDos_DoesNotReturnOtherUsers()
        {
            // Arrange
            HttpClient userA = await RegisteredUser();
            await userA.PostAsJsonAsync("/api/todo/", new
            {
                Title = "A's ToDo",
                Description = ""
            });

            HttpClient userB = await RegisteredUser();
            await userB.PostAsJsonAsync("/api/todo/", new
            {
                Title = "B's ToDo",
                Description = ""
            });

            // Act
            HttpResponseMessage response = await userA.GetAsync("/api/todo/");

            // Assert
            response.EnsureSuccessStatusCode();

            List<ToDoDto>? toDoDtos = await response.Content.ReadFromJsonAsync<List<ToDoDto>>();
            Assert.NotNull(toDoDtos);
            Assert.Equal("A's ToDo", toDoDtos[0]!.Title);
        }

        [Fact]
        public async Task GetAllToDos_WithoutUserId_ReturnsUnauthorized()
        {
            // Arrange 
            HttpClient client = factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("/api/todo/");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region GET /api/todo/{id}



        #endregion
        
        #region POST /api/todo

        

        #endregion

        #region PUT /api/todo/{id}

        

        #endregion

        #region DELETE /api/todo/{id}

        

        #endregion
    }
}