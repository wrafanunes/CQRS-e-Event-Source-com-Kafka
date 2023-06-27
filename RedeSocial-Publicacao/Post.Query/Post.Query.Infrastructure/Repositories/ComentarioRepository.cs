using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class ComentarioRepository : IComentarioRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public ComentarioRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(ComentarioEntity comentario)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Comentarios.Add(comentario);

            //aqui é usado o operador de descarte, para que nada seja retornado
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid idComentario)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            var comentario = await GetByIdAsync(idComentario);

            if (null == comentario) return;

            context.Comentarios.Remove(comentario);

            //aqui é usado o operador de descarte, para que nada seja retornado
            _ = await context.SaveChangesAsync();
        }

        public async Task<ComentarioEntity> GetByIdAsync(Guid idComentario)
        {
            //o método Include retorna as propriedades de navegação do objeto.
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Comentarios
            .FirstOrDefaultAsync(_ => _.IdComentario == idComentario);
        }

        public async Task UpdateAsync(ComentarioEntity comentario)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Comentarios.Update(comentario);

            //aqui é usado o operador de descarte, para que nada seja retornado
            _ = await context.SaveChangesAsync();
        }
    }
}