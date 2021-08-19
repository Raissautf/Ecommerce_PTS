using DBR.API.Data;
using DBR.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace DBR.API.Controllers
{
    [ApiController]
    public class CatalogoController : Controller
    {
        private readonly ApiContext _context;

        public CatalogoController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet("catalogo/produtos")]
        public async Task<IEnumerable<Produto>> ObterTodos()
        {
           var response = await _context.Produtos.AsNoTracking().ToListAsync();

            return response;
        }

        [HttpGet("catalogo/produtos/{id}")]
        public async Task<Produto> ObterPorId(Guid id)
        {
            var result = await _context.Produtos.FindAsync(id);

            return result;
        }

    }
}