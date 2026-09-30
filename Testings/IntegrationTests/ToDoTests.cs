using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using ToDoListWebApi.Dtos;

namespace Testings.IntegrationTests
{
    public class ToDoTests(ToDoListWebApiFactory factory) : IClassFixture<ToDoListWebApiFactory>
    {
        readonly string toDoRoute = "/api/todo";
        readonly string title = "Test";

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
            await userA.PostAsJsonAsync(toDoRoute, new
            {
                Title = title,
                Description = ""
            });

            HttpClient userB = await RegisteredUser();

            // Act
            HttpResponseMessage response = await userB.GetAsync(toDoRoute);

            // Assert
            response.EnsureSuccessStatusCode();

            List<ToDoDto>? toDoDtos = await response.Content.ReadFromJsonAsync<List<ToDoDto>>();
            Assert.NotNull(toDoDtos);
            Assert.Empty(toDoDtos);
        }

        [Fact]
        public async Task GetAllToDos_WithoutUserId_ReturnsUnauthorized()
        {
            // Arrange 
            HttpClient client = factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(toDoRoute);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region GET /api/todo/{id}

        [Fact]
        public async Task GetToDo_ByIdAndUserId_ReturnsToDo()
        {
            // Arrange
            HttpClient user = await RegisteredUser();
            HttpResponseMessage toDoResponse = await user.PostAsJsonAsync(toDoRoute, new
            {
                Title = title,
                Description = "",
            });

            Guid id = (await toDoResponse.Content.ReadFromJsonAsync<ToDoDto>())!.Id;

            // Act
            HttpResponseMessage response = await user.GetAsync($"{toDoRoute}/{id}");

            // Assert
            response.EnsureSuccessStatusCode();

            ToDoDto? toDoDto = await response.Content.ReadFromJsonAsync<ToDoDto>();
            Assert.NotNull(toDoDto);
            Assert.Equal(title, toDoDto!.Title);
        }

        [Fact]
        public async Task GetToDo_ByIdAndOtherUserId_ReturnsNotFound()
        {
            // Arrange
            HttpClient userA = await RegisteredUser();
            HttpResponseMessage toDoResponse = await userA.PostAsJsonAsync(toDoRoute, new
            {
                Title = title,
                Description = "",
            });

            Guid id = (await toDoResponse.Content.ReadFromJsonAsync<ToDoDto>())!.Id;
            HttpClient userB = await RegisteredUser();

            // Act
            HttpResponseMessage response = await userB.GetAsync($"{toDoRoute}/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetToDo_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            HttpClient user = await RegisteredUser();
            Guid id = Guid.NewGuid();

            // Act
            HttpResponseMessage response = await user.GetAsync($"{toDoRoute}/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion
        
        #region POST /api/todo

        [Fact]
        public async Task CreateToDo_ByUserId_ReturnsToDo()
        {
            // Arrange
            HttpClient user = await RegisteredUser();

            // Act
            HttpResponseMessage response = await user.PostAsJsonAsync(toDoRoute, new
            {
                Title = title,
                Description = ""
            });

            // Assert
            response.EnsureSuccessStatusCode();

            ToDoDto? toDoDto = await response.Content.ReadFromJsonAsync<ToDoDto>();
            Assert.NotNull(toDoDto);
            Assert.Equal(title, toDoDto!.Title);
        }

        [Fact]
        public async Task CreateToDo_WithNoUserId_ReturnsUnauthorized()
        {
            // Arrange
            HttpClient client = factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(toDoRoute, new
            {
                Title = title,
                Description = ""
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreateToDo_WithMissingTitle_ReturnsBadRequest()
        {
            // Arrange
            HttpClient user = await RegisteredUser();

            // Act
            HttpResponseMessage response = await user.PostAsJsonAsync(toDoRoute, new
            {
                Title = "",
                Description = ""
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region PUT /api/todo/{id}

        [Fact]
        public async Task UpdateToDo_ByIdAndUserId_ReturnsNoContent()
        {
            // Arrange
            string updatedTitle = "UpdatedTitle";
            HttpClient user = await RegisteredUser();
            HttpResponseMessage toDoResponse = await user.PostAsJsonAsync(toDoRoute, new
            {
                Title = title,
                Description = ""
            });

            Guid id = (await toDoResponse.Content.ReadFromJsonAsync<ToDoDto>())!.Id;

            // Act
            HttpResponseMessage response = await user.PutAsJsonAsync($"{toDoRoute}/{id}", new
            {
                Title = updatedTitle,
                Description = "",
                Complete = true
            });

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task UpdateToDo_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            string updatedTitle = "UpdatedTitle";
            HttpClient user = await RegisteredUser();

            Guid id = Guid.NewGuid();

            // Act
            HttpResponseMessage response = await user.PutAsJsonAsync($"{toDoRoute}/{id}", new
            {
                Title = updatedTitle,
                Description = "",
                Complete = true
            });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateToDo_WithMissingTitle_ReturnsBadRequest()
        {
            // Arrange
            string updatedTitle = "";
            HttpClient user = await RegisteredUser();
            HttpResponseMessage toDoResponse = await user.PostAsJsonAsync(toDoRoute, new
            {
                Title = title,
                Description = ""
            });

            Guid id = (await toDoResponse.Content.ReadFromJsonAsync<ToDoDto>())!.Id;

            // Act
            HttpResponseMessage response = await user.PutAsJsonAsync($"{toDoRoute}/{id}", new
            {
                Title = updatedTitle,
                Description = "",
                Complete = false
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateToDo_ByIdAndOtherUserId_ReturnsNotFound()
        {
            // Arrange
            string updatedTitle = "UpdatedTitle";
            HttpClient userA = await RegisteredUser();
            HttpResponseMessage toDoResponse = await userA.PostAsJsonAsync(toDoRoute, new
            {
                Title = title,
                Description = "",
            });

            Guid id = (await toDoResponse.Content.ReadFromJsonAsync<ToDoDto>())!.Id;
            HttpClient userB = await RegisteredUser();

            // Act
            HttpResponseMessage response = await userB.PutAsJsonAsync($"{toDoRoute}/{id}", new
            {
                Title = updatedTitle,
                Description = "",
                Complete = false
            });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region DELETE /api/todo/{id}

        [Fact]
        public async Task DeleteToDo_ByIdAndUserId_ReturnsNoContent()
        {
            // Arrange
            HttpClient user = await RegisteredUser();
            HttpResponseMessage toDoResponse = await user.PostAsJsonAsync(toDoRoute, new
            {
                Title = title,
                Description = ""
            });

            Guid id = (await toDoResponse.Content.ReadFromJsonAsync<ToDoDto>())!.Id;

            // Act
            HttpResponseMessage response = await user.DeleteAsync($"{toDoRoute}/{id}");

            // Assert 
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteToDo_WithNonExitstentId_ReturnsNotFound()
        {
            // Arrange
            HttpClient user = await RegisteredUser();
            Guid id = Guid.NewGuid();

            // Act
            HttpResponseMessage response = await user.DeleteAsync($"{toDoRoute}/{id}");

            // Assert 
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteToDo_ByIdAndOtherUserId_ReturnsNotFound()
        {
            // Arrange
            HttpClient userA = await RegisteredUser();
            HttpResponseMessage toDoResponse = await userA.PostAsJsonAsync(toDoRoute, new
            {
                Title = title,
                Description = "",
            });

            Guid id = (await toDoResponse.Content.ReadFromJsonAsync<ToDoDto>())!.Id;
            HttpClient userB = await RegisteredUser();

            // Act
            HttpResponseMessage response = await userB.DeleteAsync($"{toDoRoute}/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion
    }
}