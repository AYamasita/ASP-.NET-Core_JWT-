using Api_Jwt1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Jwt1.Controllers
{

    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken([FromBody] User request)
        {
            if (request.UserName == "mac" && request.Password == "numsey")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.UserName)
                    //new Claim(ClaimTypes.Role, "Admin") //poda adicionar mais regras
                };

                //Recebe um instancia da classe SymetricSecurityKey
                //Armazenando a chave de criptografica usada na criação do token

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["SecurityKey"]));

                //Recebe um objeto do tipo SigninCredentials contendo a chave de 
                //criptografia e o algoritmo de segurança empregados na geração  de assinatura
                //digitais  para tokens

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return Unauthorized();
        }
    }
}