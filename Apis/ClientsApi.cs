namespace MinimialApis.Apis;

public class ClientsApi
{
  private readonly ILogger<ClientsApi> _logger;
  private readonly IMapper _mapper;

  public ClientsApi(ILogger<ClientsApi> logger, IMapper mapper)
  {
    _logger = logger;
    _mapper = mapper;
  }

  public void Register(IEndpointRouteBuilder app)
  {
    app.MapGet("/clients", GetClients);
    app.MapGet("/clients/{id:int}", GetClients);
    app.MapPost("/clients", CreateClient);
    app.MapPut("/clients/{id}", UpdateClient);
    app.MapDelete("/clients/{id}", DeleteClient);
  }

  private async Task<IResult> GetClients(IJurisRepository repo)
  {
    var results = await repo.GetClientsAsync();
    return Results.Ok(
      _mapper.Map<IEnumerable<ClientModel>>(results));
  }

  private async Task<IResult> GetClient(int id, IJurisRepository repo)
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
        _mapper.Map<ClientModel>(client));
    }
    catch (Exception ex)
    {
      _logger.LogError(
        "Failed while reading: {message}",
        ex.Message);
    }
    return Results.BadRequest("Failed");
  }

  private async Task<IResult> CreateClient(
    ClientModel model,
    IJurisRepository repo)
  {

    try
    {
      var newClient = _mapper.Map<Client>(model);
      repo.Add(newClient);
      if (await repo.SaveAll())
      {
        return Results.Created(
          $"/clients/{newClient.Id}",
          _mapper.Map<ClientModel>(newClient));
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(
        "Failed while creating client: {ex}",
        ex);
    }
    return Results.BadRequest(
      "Failed to create client");
  }

  private async Task<IResult> UpdateClient(
    int id,
             ClientModel model,
             IJurisRepository repo)
  {
    try
    {
      var oldClient =
        await repo.GetClientAsync(id);
      if (oldClient == null)
      {
        return Results.NotFound();
      }

      _mapper.Map(model, oldClient);
      if (await repo.SaveAll())
      {
        return Results.Ok(
          _mapper.Map<ClientModel>(oldClient));
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(
        "Failed while updating client: {ex}",
        ex);
    }
    return Results.BadRequest(
      "Failed to update client");
  }

  private async Task<IResult> DeleteClient(
    int id,
    IJurisRepository repo)
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
      _logger.LogError(
        "Failed while deleting client: {ex}",
        ex);
    }
    return Results.BadRequest(
      "Failed to deleting client");
  }
}
