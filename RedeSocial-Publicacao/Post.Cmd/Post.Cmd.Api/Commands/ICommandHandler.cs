using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Post.Cmd.Api.Commands
{
    public interface ICommandHandler
    {
        Task HandleAsync(NovaPublicacaoCommand command);
        Task HandleAsync(EditarMensagemCommand command);
        Task HandleAsync(CurtirPublicacaoCommand command);
        Task HandleAsync(AdicionarComentarioCommand command);
        Task HandleAsync(EditarComentarioCommand command);
        Task HandleAsync(RemoverComentarioCommand command);
        Task HandleAsync(ExcluirPublicacaoCommand command);
        Task HandleAsync(RestoreReadDbCommand command);
    }
}