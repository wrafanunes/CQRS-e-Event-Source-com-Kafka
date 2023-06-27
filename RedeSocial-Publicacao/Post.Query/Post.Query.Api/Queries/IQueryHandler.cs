using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Queries
{
    public interface IQueryHandler
    {
        Task<List<PublicacaoEntity>> HandleAsync(BuscarTodasAsPublicacoesQuery query);
        Task<List<PublicacaoEntity>> HandleAsync(BuscarPublicacaoPorIdQuery query);
        Task<List<PublicacaoEntity>> HandleAsync(BuscarPublicacoesPeloAutorQuery query);
        Task<List<PublicacaoEntity>> HandleAsync(BuscarPublicacoesComComentariosQuery query);
        Task<List<PublicacaoEntity>> HandleAsync(BuscarPublicacoesComCurtidasQuery query);
    }
}