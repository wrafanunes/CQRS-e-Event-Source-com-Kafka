using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class AdicionarComentarioCommand : BaseCommand
    {
        public string Comentario { get; set; }
        public string NomeUsuario { get; set; }
    }
}