using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Domain;
using Post.Comon.Events;

namespace Post.Cmd.Domain.Aggregates
{
    public class PublicacaoAggregate : AggregateRoot
    {
        private bool _active;
        private string _autor;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active { get => _active; set => _active = value; }

        public PublicacaoAggregate()
        {
            
        }

        public PublicacaoAggregate(Guid id, string autor, string mensagem)
        {
            RaiseEvent(new PublicacaoCriadaEvent
            {
                Id = id,
                Autor = autor,
                Mensagem = mensagem,
                DataPublicacao = DateTime.Now
            });
        }

        public void Apply(PublicacaoCriadaEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _autor = @event.Autor;
        }

        public void EditMessage(string mensagem)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive post.");
            }

            if (string.IsNullOrWhiteSpace(mensagem))
            {
                throw new InvalidOperationException($"The value of {nameof(mensagem)} cannot be null or empty. Please, provide a valid {nameof(mensagem)}.");
            }

            RaiseEvent(new MensagemEditadaEvent
            {
                Id = _id,
                Mensagem = mensagem,
            });
        }

        public void Apply(MensagemEditadaEvent @event)
        {
            _id = @event.Id;
        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive post.");
            }

            RaiseEvent(new PublicacaoCurtidaEvent
            {
                Id = _id
            });
        }

        public void Apply(PublicacaoCurtidaEvent @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comentario, string nomeUsuario)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add a comment to an inactive post.");
            }

            if (string.IsNullOrWhiteSpace(comentario))
            {
                throw new InvalidOperationException($"The value of {nameof(comentario)} cannot be null or empty. Please, provide a valid {nameof(comentario)}.");
            }

            RaiseEvent(new ComentarioAdicionadoEvent
            {
                Id = _id,
                IdComentario = Guid.NewGuid(),
                Comentario = comentario,
                NomeUsuario = nomeUsuario,
                DataCriacaoComentario = DateTime.Now
            });
        }

        public void Apply(ComentarioAdicionadoEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.IdComentario, new Tuple<string, string>(@event.Comentario, @event.NomeUsuario));
        }

        public void EditComment(Guid idComentario, string comentario, string nomeUsuario)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit a comment of an inactive post.");
            }

            if (!_comments[idComentario].Item2.Equals(nomeUsuario, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment made by another user");
            }

            RaiseEvent(new ComentarioEditadoEvent
            {
                Id = _id,
                IdComentario = idComentario,
                Comentario = comentario,
                NomeUsuario = nomeUsuario,
                DataEdicaoComentario = DateTime.Now
            });
        }

        public void Apply(ComentarioEditadoEvent @event)
        {
            _id = @event.Id;
            _comments[@event.IdComentario] = new Tuple<string, string>(@event.Comentario, @event.NomeUsuario);
        }

        public void RemoveComment(Guid idComentario, string nomeUsuario)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove a comment of an inactive post.");
            }

            if (!_comments[idComentario].Item2.Equals(nomeUsuario, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment made by another user.");
            }

            RaiseEvent(new ComentarioRemovidoEvent
            {
                Id = _id,
                IdComentario = idComentario
            });
        }

        public void Apply(ComentarioRemovidoEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.IdComentario);
        }

        public void DeletePost(string nomeUsuario)
        {
            if (!_active)
            {
                throw new InvalidOperationException("The post has already been removed");
            }

            if (!_autor.Equals(nomeUsuario, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a post made by somebody else");
            }

            RaiseEvent(new PublicacaoExcluidaEvent
            {
                Id = _id
            });
        }

        public void Apply(PublicacaoExcluidaEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }
    }
}