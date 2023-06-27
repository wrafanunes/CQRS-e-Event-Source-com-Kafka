using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Comon.Events
{
    public class PublicacaoCriadaEvent : BaseEvent
    {
        public string Autor { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataPublicacao { get; set; }

        public PublicacaoCriadaEvent() : base(nameof(PublicacaoCriadaEvent))
        {

        }
    }
}