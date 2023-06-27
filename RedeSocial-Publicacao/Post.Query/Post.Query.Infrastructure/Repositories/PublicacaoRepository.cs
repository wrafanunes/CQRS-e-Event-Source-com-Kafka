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
    public class PublicacaoRepository : IPublicacaoRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public PublicacaoRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(PublicacaoEntity publicacao)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Publicacoes.Add(publicacao);

            //aqui é usado o operador de descarte, para que nada seja retornado
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid idPublicacao)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            var publicacao = await GetByIdAsync(idPublicacao);

            if (null == publicacao) return;

            context.Publicacoes.Remove(publicacao);

            //aqui é usado o operador de descarte, para que nada seja retornado
            _ = await context.SaveChangesAsync();
        }

        public async Task<PublicacaoEntity> GetByIdAsync(Guid idPublicacao)
        {
            //o método Include retorna as propriedades de navegação do objeto.
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Publicacoes
            .Include(p => p.Comentarios)
            .FirstOrDefaultAsync(_ => _.IdPublicacao == idPublicacao);
        }

        public async Task<List<PublicacaoEntity>> ListAllAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Publicacoes.AsNoTracking()
                    .Include(p => p.Comentarios).AsNoTracking()
                    .ToListAsync();
        }

        public async Task<List<PublicacaoEntity>> ListByAuthorAsync(string autor)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Publicacoes.AsNoTracking()
                    .Include(p => p.Comentarios).AsNoTracking()
                    .Where(x => x.Autor.Contains(autor))
                    .ToListAsync();
        }

        public async Task<List<PublicacaoEntity>> ListWithCommentsAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            var teste = await context.Publicacoes.AsNoTracking()
                    .Include(p => p.Comentarios).AsNoTracking()
                    .ToListAsync();
            return await context.Publicacoes.AsNoTracking()
                    .Include(p => p.Comentarios).AsNoTracking()
                    .Where(x => x.Comentarios != null && x.Comentarios.Any())
                    .ToListAsync();
        }

        public async Task<List<PublicacaoEntity>> ListWithLikesAsync(int numeroDeCurtidas)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Publicacoes.AsNoTracking()
                    .Include(p => p.Comentarios).AsNoTracking()
                    .Where(x => x.Curtidas >= numeroDeCurtidas)
                    .ToListAsync();
        }

        public async Task UpdateAsync(PublicacaoEntity publicacao)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Publicacoes.Update(publicacao);

            //aqui é usado o operador de descarte, para que nada seja retornado
            _ = await context.SaveChangesAsync();
        }
    }
}