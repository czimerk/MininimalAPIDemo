using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TestMinApi.Commands;
using TestMinApi.Dto;
using TestMinApi.Extensions;
using TestMinApi.Queries;
using static TestMinApi.Helpers.EndpointHelper;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
// Register Serilog
builder.Logging.AddSerilog(logger);

var app = builder
    .AddJWTAuthentication()
    .AddApiServices()
    .Build();

app.ConfigureServices();

Anonymous(
    app.MapPost("/login", (IMediator m, [FromBody] LoginCommand login) => m.Send(login)).AllowAnonymous(),
    app.MapGet("/orders/{id}/status", (IMediator m, Guid id) => m.Send(new GetOrderStatusQuery(id)))
);

app.MapGet("/orders/{id}", () => { return "Order details when user is logged in"; });
app.MapGet("/orders", (IMediator m, GetOrdersQuery query) => m.Send(query));
app.MapGet("/articles", (IMediator m, GetArticleQuery query) => m.Send(query));
app.MapGet("/articles/{id}", (IMediator m, GetArticleByIdQuery query) => m.Send(query));
app.MapPost("/articles", (IMediator m, [FromBody] ArticleDto c) => m.Send(new CreateArticleCommand(c)));

Admin(
    app.MapGet("/users", () => { return "Get list of users"; }),
    app.MapGet("/users/{id}", () => { return "Get user with Id"; }),
    app.MapPost("/users", () => { return "User added"; })
);

app.Run();

