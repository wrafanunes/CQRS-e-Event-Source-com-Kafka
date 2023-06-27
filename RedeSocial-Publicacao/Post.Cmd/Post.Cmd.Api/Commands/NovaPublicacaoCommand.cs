using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class NovaPublicacaoCommand : BaseCommand
    {
        public string Autor { get; set; }
        public string Mensagem { get; set; }
    }
}