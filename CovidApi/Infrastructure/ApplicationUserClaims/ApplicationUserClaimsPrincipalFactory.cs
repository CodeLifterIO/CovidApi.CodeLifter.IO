using System.Security.Claims;
using System.Threading.Tasks;
using CovidApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CovidApi.Infrastructure.ApplicationUserClaims
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        { }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim("FirstName", user.FirstName)
                });
            }

            // You can add more properties that you want to expose on the User object below

            if (!string.IsNullOrWhiteSpace(user.MiddleName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim("MiddleName", user.MiddleName)
                });
            }

            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim("LastName", user.LastName)
                });
            }

            if (!string.IsNullOrWhiteSpace(user.Github))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim("Github", user.Github)
                });
            }

            if (!string.IsNullOrWhiteSpace(user.LinkedIn))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim("LinkedIn", user.LinkedIn)
                });
            }

            if (!string.IsNullOrWhiteSpace(user.Twitter))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim("Twitter", user.Twitter)
                });
            }

            return principal;
        }
    }
}
