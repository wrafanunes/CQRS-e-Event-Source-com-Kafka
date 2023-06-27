using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Comon.DTOs;

namespace Post.Cmd.Api.DTOs
{
    public class NovaPublicacaoResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}