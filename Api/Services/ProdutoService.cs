using Api.Context;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class ProdutoService
    {
        private readonly Contexto _context;

        public ProdutoService(Contexto context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            return await _context.Produto.ToListAsync();
        }
    }
}
