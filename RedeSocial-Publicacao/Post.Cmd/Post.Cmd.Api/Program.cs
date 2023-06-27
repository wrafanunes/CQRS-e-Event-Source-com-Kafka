using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using MongoDB.Bson.Serialization;
using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Producers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;
using Post.Comon.Events;

var builder = WebApplication.CreateBuilder(args);

BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<PublicacaoCriadaEvent>();
BsonClassMap.RegisterClassMap<MensagemEditadaEvent>();
BsonClassMap.RegisterClassMap<PublicacaoCurtidaEvent>();
BsonClassMap.RegisterClassMap<ComentarioAdicionadoEvent>();
BsonClassMap.RegisterClassMap<ComentarioEditadoEvent>();
BsonClassMap.RegisterClassMap<ComentarioRemovidoEvent>();
BsonClassMap.RegisterClassMap<PublicacaoExcluidaEvent>();

// Add services to the container.

//Aqui a ordem importa, já que cada uma das seguintes injeções de dependência configuradas depende da anterior.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
//Add scoped vai criar uma instância para cada requisição http única que é feita para a API Post.CMD.
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PublicacaoAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

//registrar métodos do command handler
var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var commandDispatcher = new CommandDispatcher();

commandDispatcher.RegisterHandler<NovaPublicacaoCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<EditarMensagemCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<CurtirPublicacaoCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<AdicionarComentarioCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<EditarComentarioCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<RemoverComentarioCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<ExcluirPublicacaoCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<RestoreReadDbCommand>(commandHandler.HandleAsync);

builder.Services.AddSingleton<ICommandDispatcher>(_ => commandDispatcher);

builder.Services.AddControllers();
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
