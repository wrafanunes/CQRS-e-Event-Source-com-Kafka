using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories
{
    public interface IComentarioRepository
    {
        Task CreateAsync(ComentarioEntity comentario);
        Task UpdateAsync(ComentarioEntity comentario);
        Task<ComentarioEntity> GetByIdAsync(Guid idComentario);
        Task DeleteAsync(Guid idComentario);
    }
}