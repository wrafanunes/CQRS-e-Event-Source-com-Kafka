using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DatabaseContextFactory
    {
        /* uma Action é um delegate (um tipo que representa uma referência para uma função, podendo usá-la como argumento para outra função) que encapsula um método que tem um
        único parâmetro mas não tem valor de retorno */
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public DatabaseContext CreateDbContext()
        {
            DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new();
            _configureDbContext(optionsBuilder);

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}