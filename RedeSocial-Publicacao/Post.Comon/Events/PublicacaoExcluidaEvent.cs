using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Comon.Events
{
    public class PublicacaoExcluidaEvent : BaseEvent
    {
        public PublicacaoExcluidaEvent() : base(nameof(PublicacaoExcluidaEvent))
        {
        }
    }
}