using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories
{
    public interface IPublicacaoRepository
    {
        Task CreateAsync(PublicacaoEntity publicacao);
        Task UpdateAsync(PublicacaoEntity publicacao);
        Task DeleteAsync(Guid idPublicacao);
        Task<PublicacaoEntity> GetByIdAsync(Guid idPublicacao);
        Task<List<PublicacaoEntity>> ListAllAsync();
        Task<List<PublicacaoEntity>> ListByAuthorAsync(string autor);
        Task<List<PublicacaoEntity>> ListWithLikesAsync(int numeroDeCurtidas);
        Task<List<PublicacaoEntity>> ListWithCommentsAsync();
    }
}