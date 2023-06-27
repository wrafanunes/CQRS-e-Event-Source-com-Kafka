using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Comon.Events
{
    public class PublicacaoCurtidaEvent : BaseEvent
    {
        public PublicacaoCurtidaEvent() : base(nameof(PublicacaoCurtidaEvent))
        {
        }
    }
}