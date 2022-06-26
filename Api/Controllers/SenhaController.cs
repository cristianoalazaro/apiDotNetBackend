using Api.Context;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Controllers
{
    [ApiController]
    [Route("password")]
    public class SenhaController : Controller
    {
        private readonly Contexto _contexto;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public SenhaController(Contexto contexto, IConfiguration configuration)
        {
            _contexto = contexto;
            _configuration = configuration;
            _emailService = new EmailService(configuration);
        }

        [HttpPost("requestchange")]
        public async Task<ActionResult> ChangePassword([FromBody] Usuario usuario)
        {
            if (usuario.Email == null)
                return BadRequest("E-mail é obrigatório!");

            var user = await _contexto.Usuario.FirstOrDefaultAsync(u => u.Email == usuario.Email);
            if (user != null)
            {
                try
                {
                _emailService.Send(usuario.Email);
                }
                catch
                {
                }
            }            

            return Ok(new { message = "E-mail enviado para o usuário com o link de troca de senha!" });
        }

    }
}
