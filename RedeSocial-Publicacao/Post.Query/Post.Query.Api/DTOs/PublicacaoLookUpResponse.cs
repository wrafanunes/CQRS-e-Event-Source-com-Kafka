using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Comon.DTOs;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.DTOs
{
    public class PublicacaoLookUpResponse : BaseResponse
    {
        public List<PublicacaoEntity> Publicacoes { get; set; }
    }
}