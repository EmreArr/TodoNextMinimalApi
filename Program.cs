using Microsoft.EntityFrameworkCore;
using MinAPISeparateFile;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDbContext<ProductDb>(opt => opt.UseInMemoryDatabase("ProductList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Swagger 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    // for the annotations
    x.EnableAnnotations();
});

var app = builder.Build();

app.Urls.Add("http://localhost:4001");

TodoEndpoints.Map(app);
ProductEndpoints.Map(app);

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.Run();
