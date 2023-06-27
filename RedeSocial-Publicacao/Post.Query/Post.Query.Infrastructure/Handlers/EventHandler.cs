using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Comon.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IPublicacaoRepository _publicacaoRepository;
        private readonly IComentarioRepository _comentarioRepository;

        public EventHandler(IPublicacaoRepository publicacaoRepository, IComentarioRepository comentarioRepository)
        {
            _publicacaoRepository = publicacaoRepository;
            _comentarioRepository = comentarioRepository;
        }

        public async Task On(PublicacaoCriadaEvent @event)
        {
            PublicacaoEntity publicacao = new()
            {
                IdPublicacao = @event.Id,
                Autor = @event.Autor,
                DataPublicacao = @event.DataPublicacao,
                Mensagem = @event.Mensagem
            };

            await _publicacaoRepository.CreateAsync(publicacao);
        }

        public async Task On(MensagemEditadaEvent @event)
        {
            var publicacao = await _publicacaoRepository.GetByIdAsync(@event.Id);

            if (null == publicacao) return;

            publicacao.Mensagem = @event.Mensagem;
            await _publicacaoRepository.UpdateAsync(publicacao);
        }

        public async Task On(PublicacaoCurtidaEvent @event)
        {
            var publicacao = await _publicacaoRepository.GetByIdAsync(@event.Id);

            if (null == publicacao) return;

            publicacao.Curtidas++;
            await _publicacaoRepository.UpdateAsync(publicacao);
        }

        public async Task On(ComentarioAdicionadoEvent @event)
        {
            ComentarioEntity comentario = new()
            {
                IdPublicacao = @event.Id,
                IdComentario = @event.IdComentario,
                DataComentario = @event.DataCriacaoComentario,
                Comentario = @event.Comentario,
                NomeUsuario = @event.NomeUsuario
            };

            await _comentarioRepository.CreateAsync(comentario);
        }

        public async Task On(ComentarioEditadoEvent @event)
        {
            var comentario = await _comentarioRepository.GetByIdAsync(@event.IdComentario);

            if (null == comentario) return;

            comentario.Comentario = @event.Comentario;
            comentario.Editado = true;
            comentario.DataComentario = @event.DataEdicaoComentario;

            await _comentarioRepository.UpdateAsync(comentario);
        }

        public async Task On(ComentarioRemovidoEvent @event)
        {
            await _comentarioRepository.DeleteAsync(@event.IdComentario);
        }

        public async Task On(PublicacaoExcluidaEvent @event)
        {
            await _publicacaoRepository.DeleteAsync(@event.Id);
        }
    }
}