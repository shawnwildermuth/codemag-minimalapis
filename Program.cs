using System.Reflection;
using AutoMapper;

var bldr = WebApplication.CreateBuilder(args);

bldr.Services.AddDbContext<JurisContext>();
bldr.Services
  .AddTransient<IJurisRepository,
                JurisRepository>();
bldr.Services
  .AddAutoMapper(Assembly.GetEntryAssembly());

var app = bldr.Build();

// Get a shared logger object
var loggerFactory =
  app.Services.GetService<ILoggerFactory>();
var logger =
  loggerFactory?.CreateLogger<Program>();

if (logger == null)
{
  throw new InvalidOperationException(
    "Logger not found");
}

// Get the Automapper, we can share this too
var mapper = app.Services.GetService<IMapper>();
if (mapper == null)
{
  throw new InvalidOperationException(
    "Mapper not found");
}

app.MapGet("/clients",
  async (IJurisRepository repo) =>
  {
    var results = await repo.GetClientsAsync();
    return mapper.Map<
      IEnumerable<ClientModel>
    >(results);
  });

app.MapGet("/clients/{id:int}",
  async (int id, IJurisRepository repo) =>
  {

    try
    {
      var client =
        await repo.GetClientAsync(id);
      if (client == null)
      {
        return Results.NotFound();
      }
      return Results.Ok(
        mapper.Map<ClientModel>(client));
    }
    catch (Exception ex)
    {
      logger.LogError(
        "Failed while reading: {message}",
        ex.Message);
    }
    return Results.BadRequest("Failed");
  });

app.MapPost("/clients",
  async (ClientModel model,
         IJurisRepository repo) =>
  {

    try
    {
      var newClient = mapper.Map<Client>(model);
      repo.Add(newClient);
      if (await repo.SaveAll())
      {
        return Results.Created(
          $"/clients/{newClient.Id}",
          mapper.Map<ClientModel>(newClient));
      }
    }
    catch (Exception ex)
    {
      logger.LogError(
        "Failed while creating client: {ex}",
        ex);
    }
    return Results.BadRequest(
      "Failed to create client");
  });

app.MapPut("/clients/{id}",
  async (int id,
         ClientModel model,
         IJurisRepository repo) =>
  {
    try
    {
      var oldClient =
        await repo.GetClientAsync(id);
      if (oldClient == null)
      {
        return Results.NotFound();
      }

      mapper.Map(model, oldClient);
      if (await repo.SaveAll())
      {
        return Results.Ok(
          mapper.Map<ClientModel>(oldClient));
      }
    }
    catch (Exception ex)
    {
      logger.LogError(
        "Failed while updating client: {ex}", 
        ex);
    }
    return Results.BadRequest(
      "Failed to update client");
  });

app.MapDelete("/clients/{id}",
  async (int id,
         IJurisRepository repo) =>
  {
    try
    {
      var client =
        await repo.GetClientAsync(id);
      if (client == null)
      {
        return Results.NotFound();
      }
      repo.Delete(client);
      if (await repo.SaveAll())
      {
        return Results.Ok();
      }
    }
    catch (Exception ex)
    {
      logger.LogError(
        "Failed while deleting client: {ex}", 
        ex);
    }
    return Results.BadRequest(
      "Failed to deleting client");
  });

app.MapGet("/clients/{id}/cases", 
  async (int id, IJurisRepository repo) => {
    var cases = await repo.GetClientCases(id);
    return Results.Ok(cases);    
  });

app.Run();
