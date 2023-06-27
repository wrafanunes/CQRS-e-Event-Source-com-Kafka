using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IPublicacaoRepository _publicacaoRepository;

        public QueryHandler(IPublicacaoRepository publicacaoRepository)
        {
            _publicacaoRepository = publicacaoRepository;
        }

        public async Task<List<PublicacaoEntity>> HandleAsync(BuscarTodasAsPublicacoesQuery query)
        {
            return await _publicacaoRepository.ListAllAsync();
        }

        public async Task<List<PublicacaoEntity>> HandleAsync(BuscarPublicacaoPorIdQuery query)
        {
            var publicacao = await _publicacaoRepository.GetByIdAsync(query.Id);
            return new List<PublicacaoEntity>() { publicacao };
        }

        public async Task<List<PublicacaoEntity>> HandleAsync(BuscarPublicacoesPeloAutorQuery query)
        {
            return await _publicacaoRepository.ListByAuthorAsync(query.Autor);
        }

        public async Task<List<PublicacaoEntity>> HandleAsync(BuscarPublicacoesComComentariosQuery query)
        {
            return await _publicacaoRepository.ListWithCommentsAsync();
        }

        public async Task<List<PublicacaoEntity>> HandleAsync(BuscarPublicacoesComCurtidasQuery query)
        {
            return await _publicacaoRepository.ListWithLikesAsync(query.NumeroDeCurtidas);
        }
    }
}