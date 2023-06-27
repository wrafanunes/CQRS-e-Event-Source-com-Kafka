using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class EditarComentarioCommand : BaseCommand
    {
        public Guid IdComentario { get; set; }
        public string Comentario { get; set; }
        public string NomeUsuario { get; set; }
    }
}