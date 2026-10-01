** Link of the Api: https://todolistwebapi.onrender.com/ **
*(Bare url won't work, try adding endpoints like /api/auth/register or as listed below)*

## ToDoListWebApi
A simple to-do list REST API built with ASP.NET Core using JWT auth, service layer and integration tests.

## Tech Stack
- ASP.NET Core
- Entity Framework Core
- Npgsql
- Supabase Postgres
- JWT Auth
- Render

## Endpoints

### Auth
- `POST /api/auth/login` : Login with email and password
- `POST /api/auth/register` : Register with name, email and password

### To-Do (Auth required)
- `GET /api/todo` : Get all todos by user id
- `GET /api/todo/{id}` : Get one todo by user id
- `POST /api/todo` : Create a todo
- `PUT /api/todo/{id}` : Update a todo
- `DELETE /api/todo/{id}` : Delete a todo

## How to Run
1. Clone the repo
2. `dotnet restore`
3. Set up user-secrets:
   ```bash
   dotnet user-secrets set "JWTAUDIENCE" "<your_audience>"
   dotnet user-secrets set "JWTISSUER" "<your_issuer>"
   dotnet user-secrets set "JWTKEY" "<your-secret>"
   dotnet user-secrets set "SUPABASE_CONNECTION_STRING" "Host=<host>;Port=5432;Database=postgres;Username=<user>;Password=<pass>"
   ```
   Issuer and audience can be the same value if you're not using a third-party auth provider. For Supabase, use the **Session Pooler** connection string, the direct one won't work on Render's free tier.
4. `dotnet ef database update`
5. `dotnet run`
6. Test with curl, Postman, or Thunder Client or any of your preferred ones

## What I'd Improve
- Add a frontend
- Implement more advanced features like refresh token, reminder, etc...
