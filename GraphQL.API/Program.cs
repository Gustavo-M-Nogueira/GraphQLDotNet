using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQL.API.DataAccess;
using GraphQL.API.DataAccess.Repository;
using GraphQL.API.DataLoader;
using GraphQL.API.GraphQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options => 
        options.UseSqlite(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddGraphQLServer()
    .RegisterDbContextFactory<ApplicationDbContext>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions()
    .AddDataLoader<InstructorDataLoader>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization();

// FireBase Auth
builder.Services.AddSingleton(FirebaseApp.Create());
builder.Services.AddFirebaseAuthentication();

builder.Services.AddAuthorization(
    o => o.AddPolicy("IsAdmin", 
        p => p.RequireClaim(FirebaseUserClaimType.EMAIL, "gustavonogueira5002@gmail.com")));

builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorsRepository>();

var app = builder.Build();

// apply migrations on start up
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>(); 
    using var db = factory.CreateDbContext(); 
    db.Database.Migrate(); 
}

app.UseRouting();
app.UseAuthentication();
app.UseWebSockets();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL("/graphql");
});

app.Run();
