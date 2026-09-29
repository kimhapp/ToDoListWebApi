namespace Testings
{
    public class AuthTests(ToDoListWebApiFactory factory) : IClassFixture<ToDoListWebApiFactory>
    {        
        private readonly HttpClient client = factory.CreateClient();
    }
}