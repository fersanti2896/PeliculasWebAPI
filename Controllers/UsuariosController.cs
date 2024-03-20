﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SPeliculasAPI.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SPeliculasAPI.Controllers {
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsuariosController : ControllerBase {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public UsuariosController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager
        ) {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Registra un usuario en nuestra API.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost("registrar")]
        public async Task<ActionResult<Autenticacion>> registrar(Usuario usuario) {
            var user = new IdentityUser { UserName = usuario.Email, Email = usuario.Email };
            var result = await userManager.CreateAsync(user, usuario.Password);

            if (result.Succeeded) { 
                return await construirToken(usuario);
            } else {
                return BadRequest(result.Errors);
            }
        }

        /// <summary>
        /// Logueo de un usuario en nuestra API.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<Autenticacion>> login(Usuario usuario) {
            var result = await signInManager.PasswordSignInAsync(usuario.Email, usuario.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded) {
                return await construirToken(usuario);
            } else {
                return BadRequest("Login incorrecto.");
            }
        }

        /// <summary>
        /// Renueva el token de acceso.
        /// </summary>
        /// <returns></returns>
        [HttpGet("renovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Autenticacion>> RenovarToken() {
            var emailClaim = HttpContext.User.Claims.Where(c => c.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;

            var usuario = new Usuario { Email = email };

            return await construirToken(usuario);
        }

        /// <summary>
        /// Asigna un rol a un usuario cuando se registra.
        /// </summary>
        /// <param name="rolDTO"></param>
        /// <returns></returns>
        [HttpPost("asignarRol")]
        public async Task<ActionResult> asignarRol(RolDTO rolDTO) { 
            var usuario = await userManager.FindByEmailAsync(rolDTO.Email);
            await userManager.AddClaimAsync(usuario, new Claim("isAdmin", "1"));

            return NoContent();
        }

        /// <summary>
        /// Remueve el rol de un usuario.
        /// </summary>
        /// <param name="rolDTO"></param>
        /// <returns></returns>
        [HttpPost("removerRol")]
        public async Task<ActionResult> removerRol(RolDTO rolDTO) {
            var usuario = await userManager.FindByEmailAsync(rolDTO.Email);
            await userManager.RemoveClaimAsync(usuario, new Claim("isAdmin", "1"));

            return NoContent();
        }

        private async Task<Autenticacion> construirToken(Usuario usuario) {
            var claims = new List<Claim>() {
                new Claim("email", usuario.Email)
            };

            var user = await userManager.FindByEmailAsync(usuario.Email);
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            var claimDB = await userManager.GetClaimsAsync(user);
            claims.AddRange(claimDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddMinutes(30);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: exp, signingCredentials: creds);

            return new Autenticacion() { Token = new JwtSecurityTokenHandler().WriteToken(securityToken), Expiracion = exp };
        }
    }
}
