using Npgsql;
using System.Data;
using GabrielesProject.MovieReviewSystem.Infrastracture;
using GabrielesProject.MovieReviewSystem.Application.Interfaces;
using GabrielesProject.MovieReviewSystem.Infrastracture.Repositories;
using GabrielesProject.MovieReviewSystem.Application.Services;
using GabrielesProject.MovieReviewSystem.Application.Validators;
using GabrielesProject.MovieReviewSystem.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);
string? dbConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=movies";

// Add services to the container.


// lower case controller names
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(dbConnectionString));
builder.Services.AddInfrastructure(dbConnectionString);

builder.Services.AddTransient<IMovieRepository, MovieRepository>();
builder.Services.AddTransient<IMovieRatingRepository, MovieRatingRepository>();
builder.Services.AddTransient<IMovieService, MovieService>();
builder.Services.AddTransient<ICommentsService, CommentsService>();
builder.Services.AddTransient<IRatingValidator, RatingValidator>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }