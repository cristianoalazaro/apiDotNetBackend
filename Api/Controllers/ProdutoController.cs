using Microsoft.AspNetCore.Mvc;
using Api.Context;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [ApiController]
    [Route("produto")]
    public class ProdutoController : Controller
    {
        private readonly Contexto _contexto;
        //private readonly ProdutoService _produtoService;

        public ProdutoController(Contexto context)
        {
            _contexto = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            return Ok(await _contexto.Produto.ToListAsync());
        }

        [HttpGet("/produto/qtd")]
        public async Task<ActionResult> GetCountProduct()
        {
            return Ok(await _contexto.Produto.CountAsync());
        }

        [HttpGet("/produto/{pageNumber}/{recordsPerPage}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Produto>>> GetPagination(int pageNumber = 1, int recordsPerPage = 3)
        {
            int totalRecords = await _contexto.Produto.CountAsync();
            int totalPages = Convert.ToInt32(Math.Ceiling(totalRecords / Convert.ToDecimal(recordsPerPage)));
            return Ok(await _contexto.Produto.OrderBy(p => p.Id).Skip(recordsPerPage * (pageNumber - 1)).Take(recordsPerPage).ToListAsync());
        }

        [HttpGet("/produto/{id}")]
        [Authorize]
        public async Task<ActionResult> GetById(int id)
        {
            var produto = await _contexto.Produto.FirstOrDefaultAsync(p => p.Id == id);
            if (produto == null)
                return NotFound(new { message = "Produto não encontrado!" });
            return Ok(produto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] Produto produto)
        {
            if (produto == null)
                return BadRequest(new { errors = "Produto não pode ser nulo" });

            var prod = await _contexto.Produto.FirstOrDefaultAsync(p => p.Nome.Equals(produto.Nome));
            if (prod != null)
                return BadRequest(new { errors = "Produto já cadastrado!" });

            await _contexto.Produto.AddAsync(produto);
            await _contexto.SaveChangesAsync();
            return Ok(new { message = "Produto cadastrado com sucesso!" });
        }

        [HttpPut("/produto/{id}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, [FromBody] Produto produto)
        {
            var produtoAlterar = await _contexto.Produto.FirstOrDefaultAsync(p => p.Id.Equals(id));
            if (produtoAlterar == null)
                return NotFound(new { errors = "Produto não encontrado" });
            else
            {
                if (produto.Nome != null)
                {
                    produtoAlterar.Nome = produto.Nome;
                    await _contexto.SaveChangesAsync();
                }

                return Ok(new { message = "Produto alterado com sucesso!" });
            }
        }

        [HttpDelete("/produto/{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var produto = await _contexto.Produto.FirstOrDefaultAsync(p => p.Id.Equals(id));
            if (produto == null)
                return NotFound(new { errors = "Produto não encontrado!" });
            try
            {
                _contexto.Produto.Remove(produto);
                await _contexto.SaveChangesAsync();
                return Ok(new { message = "Produto excluido com sucesso!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { errors = "Erro ao excluir produto!" });
            }
        }
    }
}
