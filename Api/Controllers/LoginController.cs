using Api.Context;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly Contexto _contexto;

        public LoginController(Contexto contexto)
        {
            _contexto = contexto;
        }

        [HttpPost("signin")]
        public async Task<ActionResult<dynamic>> AuthenticateAsync([FromBody] Usuario usuario)
        {
            var user = await _contexto.Usuario.FirstOrDefaultAsync(
                u => u.UserName == usuario.UserName);

            bool isValidPassword = false;

            if (user != null)
            {
            isValidPassword = BCrypt.Net.BCrypt.Verify(usuario.Password, user.Password);
            }

            if (user == null || isValidPassword == false)
                return BadRequest(new { errors = "Usuário ou senha inválidos" });

            TokenService tokenService = new TokenService();
            var token = tokenService.GenerateToken(user);

            //Ocultar a senha
            user.Password = "";

            return new
            {
                user = user,
                token = token,
                message = "Login realizado com sucesso!"
            };
        }

        [HttpPost("signup")]
        public async Task<ActionResult> Post([FromBody] Usuario user)
        {
            if (user == null)
                return BadRequest(new { errors = "Usuário não pode ser nulo" });

            if (user.UserName.Length < 5)
                return BadRequest(new { errors = "Nome deve ter no mínimo 5 caracteres" });

            if (user.Password.Length < 5)
                return BadRequest(new { errors = "Senha deve ter no mínimo 5 caracteres" });

            var findUser = await _contexto.Usuario.FirstOrDefaultAsync(u => u.UserName.Equals(user.UserName));
            if (findUser != null)
                return BadRequest(new { errors = "Usuário já existe!" });
            else
            {
                user.Password = EncriptPassword(user.Password);
                await _contexto.Usuario.AddAsync(user);
                await _contexto.SaveChangesAsync();

            }

            return Ok(new {message = "Usuário cadastrado com sucesso"});
        }

        private string EncriptPassword(string password)
        {
            int workfactor = 10; //2 ^ (10) = 1024 iterations.
            string salt = BCrypt.Net.BCrypt.GenerateSalt(workfactor);
            string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hash;
        }
    }
}
