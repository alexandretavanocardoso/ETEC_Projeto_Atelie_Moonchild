using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AtelieMoonchild.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AtelieMoonchild.Data
{
    public class AtelieMoonchildContext : IdentityDbContext<Usuario> // contexto com identity e a classe que vai representa é a usuario 
    {
        public AtelieMoonchildContext (DbContextOptions<AtelieMoonchildContext> options)
            : base(options)
        {
        }

        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<Usuario>(entity => entity.Property(m => m.NormalizedEmail).HasMaxLength(85));
            builder.Entity<Usuario>(entity => entity.Property(m => m.NormalizedUserName).HasMaxLength(85));

            builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.NormalizedName).HasMaxLength(85));

            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(85));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));

            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));

            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(85));

            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));

            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));

            // IDS dos perfis
            string ROLE_ADMIN_ID = Guid.NewGuid().ToString();
            string ROLE_CLIENTE_ID = Guid.NewGuid().ToString();

            // IDS dos usuarios
            string ADMIN_ID = Guid.NewGuid().ToString();
            string CLIENTE_ID = Guid.NewGuid().ToString();

            // Permiti cadastrar um perfil
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = ROLE_ADMIN_ID, // Chave primaria
                    Name = "Administrador",
                    NormalizedName = "ADMINISTRADOR"// Regra - fica tudo em maiusculo
                },
                new IdentityRole
                {
                    Id = ROLE_CLIENTE_ID, // Chave primaria
                    Name = "Cliente",
                    NormalizedName = "CLIENTE"// Regra - fica tudo em maiusculo
                }
                );

            // Criptografia da senha
            var hash1 = new PasswordHasher<Usuario>();
            var hash2 = new PasswordHasher<Usuario>();

            builder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = ADMIN_ID,
                    Nome = "Admin",
                    Apelido = "Admin",
                    DataNascimento = DateTime.Now, // Retorna data e hora na hora criada
                    UserName = "admin@ateliemoonchild.com.br",
                    NormalizedUserName = "ADMIN@ATELIEMOONCHILD.COM.BR",
                    Email = "admin@ateliemoonchild.com.br",
                    NormalizedEmail = "ADMIN@ATELIEMOONCHILD.COM.BR",
                    EmailConfirmed = true,
                    PasswordHash = hash1.HashPassword(null, "ateliemoonchild123456789"), // Nulo e senha que vai ser criptografada
                    SecurityStamp = hash1.GetHashCode().ToString() // Necessario para discriptografa
                },
                 new Usuario
                 {
                     Id = CLIENTE_ID,
                     Nome = "Alexandre Tavano Cardoso",
                     Apelido = "Tavano",
                     DataNascimento = Convert.ToDateTime("05/08/2002"),
                     UserName = "tavanoalexandre@outlook.com",
                     NormalizedUserName = "TAVANOALEXANDRE@OUTLOOK.COM",
                     Email = "tavanoalexandre@outlook.com",
                     NormalizedEmail = "TAVANOALEXANDRE@OUTLOOK.COM",
                     EmailConfirmed = true,
                     PasswordHash = hash2.HashPassword(null, "UserProgramador123"), // Nulo e senha que vai ser criptografada
                     SecurityStamp = hash2.GetHashCode().ToString() // Necessario para discriptografa
                 }
                );


            // Determina qual user é de qual categoria
            builder.Entity<IdentityUserRole<string>>().HasData(

                new IdentityUserRole<string>
                {
                    RoleId = ROLE_ADMIN_ID,
                    UserId = ADMIN_ID
                },
                new IdentityUserRole<string>
                {
                    RoleId = ROLE_CLIENTE_ID,
                    UserId = CLIENTE_ID
                }
                );

        }
    }
}

