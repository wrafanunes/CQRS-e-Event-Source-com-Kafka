using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Comon.Events
{
    public class MensagemEditadaEvent : BaseEvent
    {
        public string Mensagem { get; set; }
        
        public MensagemEditadaEvent() : base(nameof(MensagemEditadaEvent))
        {
        }
    }
}