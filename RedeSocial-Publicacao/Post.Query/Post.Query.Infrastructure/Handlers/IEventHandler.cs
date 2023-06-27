using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Comon.Events;

namespace Post.Query.Infrastructure.Handlers
{
    public interface IEventHandler
    {
        Task On(PublicacaoCriadaEvent @event);
        Task On(MensagemEditadaEvent @event);
        Task On(PublicacaoCurtidaEvent @event);
        Task On(ComentarioAdicionadoEvent @event);
        Task On(ComentarioEditadoEvent @event);
        Task On(ComentarioRemovidoEvent @event);
        Task On(PublicacaoExcluidaEvent @event);
    }
}