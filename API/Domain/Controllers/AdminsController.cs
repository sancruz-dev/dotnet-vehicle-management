using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Enums;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.ModelViews;
using minimal_api.infrastructure.Auth;
using BCrypt.Net;

namespace minimal_api.Domain.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AdminsController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly JwtSettings _jwtSettings;

        public AdminsController(IAdminService adminService, IOptions<JwtSettings> jwtOptions)
        {
            _adminService = adminService;
            _jwtSettings = jwtOptions.Value;
        }

        string GeraTokenJwt(Admin admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("Email", admin.Email),
                new Claim("Perfil", admin.Perfil),
                new Claim(ClaimTypes.Role, admin.Perfil)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult PostLogin([FromBody] LoginDTO loginDTO)
        {
            var admin = _adminService.Login(loginDTO);
            if (admin != null)
            {
                string token = GeraTokenJwt(admin);
                return Ok(new AdminLogadoModelView
                {
                    Email = admin.Email,
                    Perfil = admin.Perfil,
                    Token = token
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("admin")]
        [Authorize(Roles = "Adm")]
        public IActionResult CadastraAdmin([FromBody] AdminDTO adminDTO)
        {
            var validacao = new ErrosValidacao();

            if (string.IsNullOrEmpty(adminDTO.Email))
                validacao.Mensagens.Add("Email não pode ser vazio");
            if (string.IsNullOrEmpty(adminDTO.Senha))
                validacao.Mensagens.Add("Senha não pode ser vazia");
            if (adminDTO.Perfil == null)
                validacao.Mensagens.Add("Perfil não pode ser vazio");


            if (validacao.Mensagens.Count() > 0)
                return BadRequest(adminDTO);

            var admin = new Admin
            {
                Email = adminDTO.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(adminDTO.Senha),
                Perfil = adminDTO.Perfil.ToString() ?? Perfil.Editor.ToString()
            };
            _adminService.Incluir(admin);

            return Created($"/admin/{admin.Id}", new AdminModelView
            {
                Id = admin.Id,
                Email = admin.Email,
                Perfil = admin.Perfil
            });
        }

        [HttpGet("admins")]
        [Authorize(Roles = "Adm")]
        public IActionResult GetAllAdmin([FromQuery] int? pagina)
        {
            var responseAdmins = new List<AdminModelView>();
            var admins = _adminService.Todos(pagina);

            foreach (var admin in admins)
            {
                responseAdmins.Add(new AdminModelView
                {
                    Id = admin.Id,
                    Email = admin.Email,
                    Perfil = admin.Perfil
                });
            }
            return Ok(responseAdmins);
        }


        [HttpGet("admins/{id}")]
        [Authorize(Roles = "Adm")]
        public IActionResult GetOneAdmins([FromRoute] int id)
        {
            var admin = _adminService.BuscaPorId(id);

            if (admin == null) return NotFound();

            return Ok(new AdminModelView
            {
                Id = admin.Id,
                Email = admin.Email,
                Perfil = admin.Perfil
            });
        }


    }
}