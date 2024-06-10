using Microsoft.EntityFrameworkCore;
using talentX.WebScrapper.Allabolog.Repositories.Classes;
using talentX.WebScrapper.Allabolog.Repositories.Contracts;
using talentX.WebScrapper.Allabolog.Repositories.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("talentX.WebScrapper.Allabolog.Api"));
});
builder.Services.AddScoped<IScrapDataRepo, ScrapDataRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
try
{
    context.Database.Migrate();
    DbInitializer.Initialize(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message, "A problem occured during migration");
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(opt =>
{
    app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));
});

app.Run();
