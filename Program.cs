var bldr = WebApplication.CreateBuilder(args);

bldr.Services.AddDbContext<JurisContext>();
bldr.Services.AddTransient<IJurisRepository, JurisRepository>();

var app = bldr.Build();

var loggerFactory = app.Services.GetService<ILoggerFactory>();
var logger = loggerFactory?.CreateLogger<Program>();

app.MapGet("/clients", 
  async (IJurisRepository repo) => {
    return await repo.GetClientsAsync();
  });

app.MapGet("/clients/{id:int}", 
  async (int id, IJurisRepository repo) => {

    try {
      var client = await repo.GetClientAsync(id);
      if (client == null) return Results.NotFound();
      return Results.Ok(client);
    }
    catch (Exception ex)
    {
      logger?.LogError("Failed while reading database: {message}", ex.Message);
    }
    return Results.BadRequest("Failed");
  });

app.Run(); 
