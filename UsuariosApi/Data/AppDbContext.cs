using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Models;
using UsuariosApi.Utils;

namespace UsuariosApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var tableIdentifier = StoreObjectIdentifier.Create(entity, StoreObjectType.Table);
                entity.SetTableName(entity.GetTableName().ToSnakeCase());
          
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName(tableIdentifier.Value).ToSnakeCase());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
                }
            }


            builder.Entity<User>().HasKey(u => u.UserId);
            builder.Entity<User>().HasIndex(u => u.Username);

        }
    }
}
