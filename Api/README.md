# ToDoListWebApi

A simple to-do list REST API built with ASP.NET Core.

## Tech Stack
- ASP.NET Core
- Entity Framework Core
- SQLite
- Swagger

## Endpoints

- `GET /api/todo` : Get all todos
- `GET /api/todo/{id}` : Get one todo
- `POST /api/todo` : Create a todo
- `PUT /api/todo/{id}` : Update a todo
- `DELETE /api/todo/{id}` : Delete a todo

## How to Run
1. Clone the repo
2. `dotnet restore`
3. `dotnet ef database update`
4. `dotnet run`
5. Open `/swagger` in your browser

## What I'd Improve
- Add authentication (JWT)
- Fetch only the current user's to-dos
- Add validation
- Add a frontend
- Add tests
