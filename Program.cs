using Microsoft.EntityFrameworkCore;
using MinAPISeparateFile;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDbContext<ProductDb>(opt => opt.UseInMemoryDatabase("ProductList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.Urls.Add("http://localhost:4001");

TodoEndpoints.Map(app);
ProductEndpoints.Map(app);

app.Run();
