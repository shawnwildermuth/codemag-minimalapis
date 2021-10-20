using System.Reflection;
using AutoMapper;
using MinimialApis.Apis;

var bldr = WebApplication.CreateBuilder(args);

RegisterServices(bldr.Services);

var app = bldr.Build();

foreach (var api in app.Services.GetServices<IApi>())
{
  api.Register(app);
}

app.Run();

void RegisterServices(IServiceCollection svc)
{
  svc.AddDbContext<JurisContext>();
  svc.AddTransient<IJurisRepository,
                  JurisRepository>();
  svc.AddAutoMapper(Assembly.GetEntryAssembly());

  var apiTypes = Assembly.GetCallingAssembly()
    .DefinedTypes.Where(t =>
      typeof(IApi)
        .IsAssignableFrom(t) &&
        !t.IsInterface &&
        !t.IsAbstract)
    .ToList();

  foreach (var theType in apiTypes)
  {
    svc.AddTransient(typeof(IApi), theType);
  }

}
