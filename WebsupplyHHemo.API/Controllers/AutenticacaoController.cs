using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebsupplyHHemo.API.ADO;
using WebsupplyHHemo.API.Attributes;
using WebsupplyHHemo.API.Models;
using WebsupplyHHemo.Interface.Funcoes;
using WebsupplyHHemo.Interface.Model;

namespace WebsupplyHHemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _ambiente;
        private static int _transacao;
        private static int _servico;
        private static string _identificador;

        public AutenticacaoController(IConfiguration configuration)
        {
            _configuration = configuration;
            _ambiente = _configuration.GetValue<string>("Parametros:Ambiente");
        }

        [HttpPost]
        [Route("login")]
        [Servico(16)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Instancia o obj do Log
            Class_Log_Hhemo objLog;

            // Instancia a Model de Log
            LogWebService logWebService;

            try
            {
                // Pega o Atributo de Serviço
                var servicoAttribute = (ServicoAttribute)Attribute.GetCustomAttribute(
                    typeof(AutenticacaoController).GetMethod(nameof(Login)),
                    typeof(ServicoAttribute));

                // Seta os parametros inicias do Log
                _transacao = 0;
                _servico = servicoAttribute.IDServico;
                _identificador = "Login" + Mod_Gerais.RetornaIdentificador();

                // Gera Log
                objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                 0, 0, JsonConvert.SerializeObject(model), null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Atualiza as Tentativas de Login
                int numTentativas = UserADO.ATUALIZA_TENTATIVA_LOGIN(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), model.Login, _configuration.GetValue<string>("Parametros:CGCMATRIZ"));

                // Consulta os Dados do Usuário
                UserModel objUser = UserADO.VALIDA_LOGIN_SEL(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), model.Login, model.Senha, _configuration.GetValue<string>("Parametros:CGCMATRIZ"));

                // Verifica se as tentativas não foram excedidas
                if (numTentativas >= 5)
                {
                    // Instancia a model do Log
                    logWebService = new LogWebService()
                    {
                        Mensagem = "Usuário bloqueado por excesso de Tentativas.",
                        Retorno = objUser
                    };

                    // Gera Log
                    objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                     0, 0, JsonConvert.SerializeObject(logWebService), null, "Erro na Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    return Unauthorized(new
                    {
                        Mensagem = logWebService.Mensagem
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

                    // Instancia a model do Log
                    logWebService = new LogWebService()
                    {
                        Mensagem = "Autenticação realizada com sucesso",
                        Retorno = objUser
                    };

                    // Gera Log
                    objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                     0, 0, JsonConvert.SerializeObject(logWebService), null, "Retorno da Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = refreshToken
                    });
                }

                // Instancia a model do Log
                logWebService = new LogWebService()
                {
                    Mensagem = "Dados de acesso incorreto ou usuário não existe na base de dados.",
                    Retorno = objUser
                };

                // Gera Log
                objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                 0, 0, JsonConvert.SerializeObject(logWebService), null, "Erro na Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                return Unauthorized(new
                {
                    Mensagem = logWebService.Mensagem
                });
            }
            catch (Exception ex)
            {
                // Inicializa a Model de Excepetion
                ExcepetionModel excepetionEstruturada = new ExcepetionModel(ex, true);

                // Gera Log
                objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                 1, -1, JsonConvert.SerializeObject(excepetionEstruturada), null, excepetionEstruturada.Mensagem,
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Devolve o Erro
                return new ObjectResult(new
                {
                    Mensagem = excepetionEstruturada.Mensagem
                })
                { StatusCode = 500 };
            }
        }


        [HttpPost]
        [Route("refresh-token")]
        [Servico(17)]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            // Instancia o obj do Log
            Class_Log_Hhemo objLog;

            // Instancia a Model de Log
            LogWebService logWebService;

            try
            {
                // Pega o Atributo de Serviço
                var servicoAttribute = (ServicoAttribute)Attribute.GetCustomAttribute(
                    typeof(AutenticacaoController).GetMethod(nameof(RefreshToken)),
                    typeof(ServicoAttribute));

                // Seta os parametros inicias do Log
                _transacao = 0;
                _servico = servicoAttribute.IDServico;
                _identificador = "Refresh" + Mod_Gerais.RetornaIdentificador();

                // Gera Log
                objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                 0, 0, JsonConvert.SerializeObject(tokenModel), null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                if (tokenModel is null)
                {
                    // Instancia a model do Log
                    logWebService = new LogWebService()
                    {
                        Mensagem = "Requisição inválida do cliente",
                        Retorno = tokenModel
                    };

                    // Gera Log
                    objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                     0, 0, JsonConvert.SerializeObject(logWebService), null, "Erro na Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    return BadRequest(logWebService.Mensagem);
                }

                string? accessToken = tokenModel.AccessToken;
                string? refreshToken = tokenModel.RefreshToken;

                var principal = GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    // Instancia a model do Log
                    logWebService = new LogWebService()
                    {
                        Mensagem = "Access token/Refresh token inválido(s)",
                        Retorno = tokenModel
                    };

                    // Gera Log
                    objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                     0, 0, JsonConvert.SerializeObject(logWebService), null, "Erro na Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    return BadRequest(logWebService.Mensagem);
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
                    // Instancia a model do Log
                    logWebService = new LogWebService()
                    {
                        Mensagem = "Access token/Refresh token inválido(s)",
                        Retorno = objUser
                    };

                    // Gera Log
                    objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                     0, 0, JsonConvert.SerializeObject(logWebService), null, "erro na Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    return BadRequest(logWebService.Mensagem);
                }

                var newAccessToken = CreateToken(principal.Claims.ToList());
                var newRefreshToken = GenerateRefreshToken();

                objUser.RefreshToken = newRefreshToken;

                TokenADO.TOKEN_INSUPD(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objUser);

                // Instancia a model do Log
                logWebService = new LogWebService()
                {
                    Mensagem = "Renovação de Token realizada com sucesso",
                    Retorno = objUser
                };

                // Gera Log
                objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                 0, 0, JsonConvert.SerializeObject(logWebService), null, "Retorno da Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                return new ObjectResult(new
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                    refreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                // Inicializa a Model de Excepetion
                ExcepetionModel excepetionEstruturada = new ExcepetionModel(ex, true);

                // Gera Log
                objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                 1, -1, JsonConvert.SerializeObject(excepetionEstruturada), null, excepetionEstruturada.Mensagem,
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Devolve o Erro
                return new ObjectResult(new
                {
                    Mensagem = excepetionEstruturada.Mensagem
                })
                { StatusCode = 500 };
            }
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
