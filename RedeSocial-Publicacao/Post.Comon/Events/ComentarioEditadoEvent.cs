using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Comon.Events
{
    public class ComentarioEditadoEvent : BaseEvent
    {
        public Guid IdComentario { get; set; }
        public string Comentario { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime DataEdicaoComentario { get; set; }

        public ComentarioEditadoEvent() : base(nameof(ComentarioEditadoEvent))
        {
        }
    }
}