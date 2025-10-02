using AlphaProject.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AlphaProject.API.Controllers
{
    /// <summary>
    /// Controller per le operazioni di autenticazione.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Endpoint per autenticare un utente e generare un token JWT.
        /// </summary>
        /// <param name="request">Credenziali di accesso.</param>
        /// <returns>Token JWT in caso di successo.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            // TODO: sostituire con la logica di autenticazione reale (es. verifica su database)
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Credenziali non valide.");
            }

            // Esempio di autenticazione fittizia: accetta solo utente 'admin' e password 'password'
            if (!(request.Username.Equals("admin", StringComparison.OrdinalIgnoreCase) && request.Password == "password"))
            {
                return Unauthorized("Credenziali errate.");
            }

            // Crea le claim del token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Recupera la chiave segreta e crea le credenziali di firma
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddHours(1);

            // Crea il token JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = jwtToken,
                expiration = expires
            });
        }
    }
}
