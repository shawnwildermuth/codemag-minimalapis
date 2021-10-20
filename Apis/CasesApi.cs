namespace MinimialApis.Apis;

public class CasesApi
{
  public void Register(IEndpointRouteBuilder app)
  {
    app.MapGet("/clients/{id}/cases", GetCases);
  }

  private async Task<IResult> GetCases(
    int id,
    IJurisRepository repo)
  {
    var cases = await repo.GetClientCases(id);
    return Results.Ok(cases);
  }

}

