using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Dispatchers;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;
using EventHandler = Post.Query.Infrastructure.Handlers.EventHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// O LazyLoading nos permite retornas nossas propriedades de navegação com nossa entidade principal, ou seja, será possível retornar os comentários em uma publicação.
Action<DbContextOptionsBuilder> configureDbContext = (_ => _.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

//Cria banco de dados e tabelas do código
DatabaseContext? dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
/* O EnsureCreated garante que o bando de dados e as tabelas foram criados. O banco de dados será criado a partir do valor da ConnectionString e as tabelas a partir dos valores DBSet
da classe DatabaseContext */
dataContext.Database.EnsureCreated();

builder.Services.AddScoped<IPublicacaoRepository, PublicacaoRepository>();
builder.Services.AddScoped<IComentarioRepository, ComentarioRepository>();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();
builder.Services.AddScoped<IEventHandler, EventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();

// registrar query handlers
var queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
var queryDispatcher = new QueryDispatcher();

queryDispatcher.RegisterHandler<BuscarTodasAsPublicacoesQuery>(queryHandler.HandleAsync);
queryDispatcher.RegisterHandler<BuscarPublicacaoPorIdQuery>(queryHandler.HandleAsync);
queryDispatcher.RegisterHandler<BuscarPublicacoesPeloAutorQuery>(queryHandler.HandleAsync);
queryDispatcher.RegisterHandler<BuscarPublicacoesComComentariosQuery>(queryHandler.HandleAsync);
queryDispatcher.RegisterHandler<BuscarPublicacoesComCurtidasQuery>(queryHandler.HandleAsync);

builder.Services.AddSingleton<IQueryDispatcher<PublicacaoEntity>>(_ => queryDispatcher);

builder.Services.AddControllers();
builder.Services.AddHostedService<ConsumerHostedService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
