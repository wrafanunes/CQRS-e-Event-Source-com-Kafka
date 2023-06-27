using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourcingHandler<PublicacaoAggregate> _eventSourcingHandler;

        public CommandHandler(IEventSourcingHandler<PublicacaoAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async Task HandleAsync(NovaPublicacaoCommand command)
        {
            PublicacaoAggregate aggregate = new(command.Id, command.Autor, command.Mensagem);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditarMensagemCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditMessage(command.Mensagem);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(CurtirPublicacaoCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(AdicionarComentarioCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.AddComment(command.Comentario, command.NomeUsuario);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditarComentarioCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditComment(command.IdComentario, command.Comentario, command.NomeUsuario);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemoverComentarioCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.RemoveComment(command.IdComentario, command.NomeUsuario);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(ExcluirPublicacaoCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.DeletePost(command.NomeUsuario);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RestoreReadDbCommand command)
        {
            await _eventSourcingHandler.RepublishEventsAsync();
        }
    }
}