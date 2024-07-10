using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebsupplyHHemo.API.ADO;
using WebsupplyHHemo.API.Models;

namespace WebsupplyHHemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AutenticacaoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Atualiza as Tentativas de Login
            int numTentativas = UserADO.ATUALIZA_TENTATIVA_LOGIN(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), model.Login, _configuration.GetValue<string>("Parametros:CGCMATRIZ"));

            // Consulta os Dados do Usuário
            UserModel objUser = UserADO.VALIDA_LOGIN_SEL(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), model.Login, model.Senha, _configuration.GetValue<string>("Parametros:CGCMATRIZ"));

            // Verifica se as tentativas não foram excedidas
            if (numTentativas >= 5)
            {
                return Unauthorized(new
                {
                    Mensagem = "Usuário bloqueado por excesso de Tentativas."
                });
            }

            if (objUser.CGC != null)
            {
                // Limpa as Tentativas
                UserADO.DESTRAVA_LOGIN(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), model.Login, _configuration.GetValue<string>("Parametros:CGCMATRIZ"));

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Login),
                    new Claim("LOGIN",model.Login),
                    new Claim("CGC",objUser.CGC),
                    new Claim("CCUSTO",objUser.CCUSTO),
                    new Claim("REQUISIT",objUser.REQUISIT),
                    new Claim("NOME",objUser.NOME),
                    new Claim("EMAIL",objUser.EMAIL),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                    out int refreshTokenValidityInMinutes);

                objUser.RefreshToken = refreshToken;
                objUser.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                TokenADO.TOKEN_INSUPD(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objUser);

                // Gera o Log de Operação
                LogDeOperacaoModel objLog = new LogDeOperacaoModel();
                objLog.nCod_Operacao = _configuration.GetValue<int>("Parametros:LogDeOperacao:Operacoes_Tipos-Codigo_Log_Token_API");
                objLog.cDetalhe = "Geração de Token e RefreshToken - API";
                objLog.cIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                LogsADO.GERA_LOGDEOPERACAO(
                    _configuration.GetValue<string>("ConnectionStrings:DefaultConnection"),
                    objUser,
                    objLog
                );

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken
                });
            }

            return Unauthorized(new
            {
                Mensagem = "Dados de acesso incorreto ou usuário não existe na base de dados."
            });
        }


        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            UserModel objUser = new UserModel();
            foreach (Claim ci in principal.Claims)
            {
                if (ci.Type == "CGC")
                    objUser.CGC = ci.Value;
                if (ci.Type == "CCUSTO")
                    objUser.CCUSTO = ci.Value;
                if (ci.Type == "REQUISIT")
                    objUser.REQUISIT = ci.Value;

            }

            objUser = TokenADO.TOKEN_SEL(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objUser);

            if (objUser == null || objUser.RefreshToken != refreshToken ||
                       objUser.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            objUser.RefreshToken = newRefreshToken;

            TokenADO.TOKEN_INSUPD(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objUser);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                 .GetBytes(_configuration["JWT:SecretKey"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out
                int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                   .GetBytes(_configuration["JWT:SecretKey"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();


            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                            out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                      !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                     StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
