using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Comon.Events
{
    public class ComentarioAdicionadoEvent : BaseEvent
    {
        public Guid IdComentario { get; set; }
        public string Comentario { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime DataCriacaoComentario { get; set; }

        public ComentarioAdicionadoEvent() : base(nameof(ComentarioAdicionadoEvent))
        {
        }
    }
}