using System.Security.Claims;
using WebsupplyHHemo.API.Models;

namespace WebsupplyHHemo.API.Helpers
{
    public class HelperClaims
    {
        public static UserModel CarregarUsuario(ClaimsIdentity identity)
        {
            UserModel objUser = new UserModel();

            IEnumerable<Claim> claims = identity.Claims;
            foreach (Claim claim in claims)
            {
                if (claim.Type == "CGC")
                    objUser.CGC = claim.Value;
                if (claim.Type == "CCUSTO")
                    objUser.CCUSTO = claim.Value;
                if (claim.Type == "REQUISIT")
                    objUser.REQUISIT = claim.Value;
                if (claim.Type == "LOGIN")
                    objUser.LOGIN = claim.Value;
                if (claim.Type == "NOME")
                    objUser.NOME = claim.Value;
                if (claim.Type == "EMAIL")
                    objUser.EMAIL = claim.Value;
            }

            return objUser;
        }
    }
}
