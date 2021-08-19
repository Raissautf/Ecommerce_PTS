using System.Threading.Tasks;
using DBR.API.Data;
using DBR.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBR.API.Controllers
{
    public class ComprasController : Controller
    {
        private readonly ApiContext _context;

        public ComprasController(ApiContext context)
        {
            _context = context;
        }

        private async Task<CarrinhoCliente> ObterCarrinho(string login)
        {
            var cliente = await _context.Pessoas.FirstOrDefaultAsync(p => p.Login == login);
            var carrinho = await _context.CarrinhoCliente
                          .Include(c => c.Itens).FirstOrDefaultAsync(x => x.ClienteId == cliente.Id);

            return carrinho;
        }
    }
}