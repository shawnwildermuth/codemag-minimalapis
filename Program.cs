using System.Reflection;
using AutoMapper;
using MinimialApis.Apis;

var bldr = WebApplication.CreateBuilder(args);

RegisterServices(bldr.Services);

var app = bldr.Build();

app.Services.GetRequiredService<ClientsApi>().Register(app);
app.Services.GetRequiredService<CasesApi>().Register(app);

app.Run();


void RegisterServices(IServiceCollection svc)
{
  svc.AddDbContext<JurisContext>();
  svc.AddTransient<IJurisRepository,
                  JurisRepository>();
  svc.AddAutoMapper(Assembly.GetEntryAssembly());

  svc.AddTransient<ClientsApi>();
  svc.AddTransient<CasesApi>();

}
