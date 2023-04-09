using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RedeSocial.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeSocial.SERVICE
{
    internal class RedeSocialDbContextFactory : IDesignTimeDbContextFactory<RedeSocialContext>
    {
        public RedeSocialContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RedeSocialContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database= RedeSocial_Database");

            return new RedeSocialContext(optionsBuilder.Options);

        }
    }
}
