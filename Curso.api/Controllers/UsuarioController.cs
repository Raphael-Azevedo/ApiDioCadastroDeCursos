using Curso.api.Business.Entities;
using Curso.api.Business.Repositories;
using Curso.api.Configurations;
using Curso.api.Filters;
using Curso.api.Infraestruture.Data;
using Curso.api.Infraestruture.Data.Repositories;
using Curso.api.Models;
using Curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Curso.api.Controllers
{
    [Route("api/v1/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;       
        private readonly IAuthenticationService _autenticationService;
        public UsuarioController(
            IUsuarioRepository usuarioRepository, 
            IAuthenticationService autenticationService)
        {
            _usuarioRepository = usuarioRepository;
            _autenticationService = autenticationService;
        }

      
        /// <summary>
        /// Este serviço permite autenticar um usuário cadastrado e ativo.
        /// </summary>
        /// <param name="loginViewModelInput">View model do login</param>
        /// <returns>Retorna status ok, dados do usuario e o token em caso de sucesso</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar usuario", Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]
        [HttpPost]
        [Route("logar")]
        [ValidacaoModelStateCustomizado]
        public IActionResult Logar(LoginViewModelInput loginViewModelInput)
        {
          var usuario = _usuarioRepository.ObterUsuario(loginViewModelInput.Login);

          if (usuario == null)
            {
                return BadRequest("Houve um erro ao tentar acessar.");
            }
       

            var usuarioViewModelOutput = new UsuarioViewModelOutput()
            {
                Codigo = usuario.Codigo,
                Login = loginViewModelInput.Login,
                Email = usuario.Email
            };
            var token = _autenticationService.GerarToken(usuarioViewModelOutput);
           
            return Ok(new
            {
                Token = token,
                Usuario = usuarioViewModelOutput
            });
        }

        /// <summary>
        /// Este serviço permite cadastrar um usuário cadastrado não existente.
        /// </summary>
        /// <param name="loginViewModelInput">View model do registro de login</param>
        [SwaggerResponse(statusCode: 201, description: "Sucesso ao cadastrar", Type = typeof(RegistroViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]
        [HttpPost]
        [Route("registrar")]
        [ValidacaoModelStateCustomizado]
        public IActionResult Registrar(RegistroViewModelInput loginViewModelInput)
        {
            try
            {
                var usuario = _usuarioRepository.ObterUsuario(loginViewModelInput.Login);

                if (usuario != null)
                {
                    return BadRequest("Usuário já cadastrado");
                }

                usuario = new Usuario
                {
                    Login = loginViewModelInput.Login,
                    Senha = loginViewModelInput.Senha,
                    Email = loginViewModelInput.Email
                };
                _usuarioRepository.Adicionar(usuario);
                _usuarioRepository.Commit();

                return Created("", loginViewModelInput);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }

        }
    }
}
