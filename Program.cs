using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Optional: Configure JSON options (you can skip this if default is fine)
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();

// Simulated session store (in-memory)
var activeUsers = new HashSet<string>();

// Public Home endpoint
app.MapGet("/home", () =>
{
    return Results.Ok("Welcome to our E-Commerce API! Shop deals, discover products, and more.");
});

// Basic Login endpoint
app.MapPost("/login", ([FromBody] LoginRequest login) =>
{
    if (login.Username == "customer" && login.Password == "shop123")
    {
        activeUsers.Add(login.Username);
        return Results.Ok("Login successful. Start exploring our shop!");
    }

    return Results.Unauthorized();
});

// Protected Dashboard endpoint
app.MapGet("/dashboard", (HttpContext context) =>
{
    var user = context.Request.Headers["X-User"].FirstOrDefault();

    if (!string.IsNullOrEmpty(user) && activeUsers.Contains(user))
    {
        return Results.Ok($"Welcome back, {user}. Here's your dashboard: orders, wishlist, and recommendations.");
    }

    return Results.Unauthorized();
});

app.Run();

// DTO record for login
public record LoginRequest(string Username, string Password);