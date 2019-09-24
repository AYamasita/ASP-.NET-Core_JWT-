using Api_Jwt1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Api_Jwt1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegurancaController : Controller
    {
        private readonly IConfiguration _config;

        public SegurancaController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult Login([FromBody] User loginDetails)
        {
            bool resultado = ValidarUsuario(loginDetails);
            if (resultado)
            {
                var tokenString = GeraTokenJWT();
                return Ok(new { token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        private String GeraTokenJWT()
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiry = DateTime.Now.AddMinutes(60);
            var securityKey = new SymmetricSecurityKey
                              (Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials
                              (securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: issuer,
                                             audience: audience,
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        private bool ValidarUsuario(User loginDetails)
        {
            if (loginDetails.UserName == "mac" && loginDetails.Password == "numsey")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}