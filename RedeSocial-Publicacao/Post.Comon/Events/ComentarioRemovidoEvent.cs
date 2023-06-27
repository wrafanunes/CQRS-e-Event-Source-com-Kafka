using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Comon.Events
{
    public class ComentarioRemovidoEvent : BaseEvent
    {
        public Guid IdComentario { get; set; }
        public ComentarioRemovidoEvent() : base(nameof(ComentarioRemovidoEvent))
        {
        }
    }
}