using AtelieMoonchild.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AtelieMoonchild.Data
{
    // Add Startup
    public class CustomClaimsFactory : UserClaimsPrincipalFactory<Usuario>
    {
        // Injeção, fuinciona usando o userManager e Options
        public CustomClaimsFactory(UserManager<Usuario> userManager,
        IOptions<IdentityOptions> optionsAcessor) : base(userManager, optionsAcessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Usuario user)
        {
            var roles = await UserManager.GetRolesAsync(user);
            user.Roles = string.Join(", ", roles); // Join pega a lista e junta todos elementos
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Nome", user.Nome));
            identity.AddClaim(new Claim("Apelido", user.Apelido));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Roles)); // Importante para ver qual o perfil, admin cliente
            return identity;
        }
    }



}
