using DBR.API.Data;
using DBR.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DBR.API.Controllers
{
    [ApiController]
    public class CarrinhoController : Controller
    {
        // contexto, classe usada para comunicação com banco de dados, EntityFrameworkCore
        private readonly ApiContext _context;

        public CarrinhoController(ApiContext context)
        {
            _context = context;
        }


        [HttpGet("carrinho/obtercarrinho/{login}")]
        public async Task<CarrinhoCliente> ObterCarrinhoCliente(string login)
        {
            //Obtém o carrinho do Cliente


            var cliente = await _context.Pessoas.FirstOrDefaultAsync(p => p.Login == login);

            var carrinho = await _context.CarrinhoCliente
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.ClienteId == cliente.Id);

            if (carrinho != null)
                return carrinho;

            return new CarrinhoCliente();
        }

        [HttpPost("carrinho/adicionar/{login}")]
        public async Task<StatusCodeResult> AdicionarItemCarrinho(string login, CarrinhoItem itemCarrinho)
        {

            /*
             Adicionar item ao carrinho, validando se o carrinho é novo ou não
             */

            var cliente = await _context.Pessoas.FirstOrDefaultAsync(p => p.Login == login);
            var carrinho = await _context.CarrinhoCliente
                                      .Include(c => c.Itens).FirstOrDefaultAsync(x => x.ClienteId == cliente.Id);

            if(carrinho == null)
            {
                carrinho = new CarrinhoCliente();
                carrinho.ClienteId = cliente.Id;
                carrinho.Itens.Add(itemCarrinho);

                decimal total = 0;

                foreach (CarrinhoItem item in carrinho.Itens)
                {
                    total += item.Quantidade * item.Valor;
                }

                carrinho.ValorTotal = total;

                _context.CarrinhoCliente.Add(carrinho);
                await _context.SaveChangesAsync();

                return StatusCode(200);
            }
            else
            {
                var itemExistente = carrinho.Itens.FirstOrDefault(c => c.ProdutoId == itemCarrinho.ProdutoId);
                if(itemExistente != null)
                {
                    var itemNovo = itemExistente;
                    carrinho.Itens.Remove(itemExistente);

                    itemNovo.Quantidade += itemCarrinho.Quantidade;
                    carrinho.Itens.Add(itemNovo);
                }
                else
                {
                    carrinho.Itens.Add(itemCarrinho);
                }

                decimal total = 0;

                foreach (CarrinhoItem item in carrinho.Itens)
                {
                    total += item.Quantidade * item.Valor;
                }

                carrinho.ValorTotal = total;

                if (itemExistente != null)
                {
                    var itemDoCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProdutoId == itemExistente.ProdutoId);
                    _context.CarrinhoItem.Update(itemDoCarrinho);
                }
                else
                {
                    _context.CarrinhoItem.Add(itemCarrinho);
                }

                _context.CarrinhoCliente.Update(carrinho);

                await _context.SaveChangesAsync();

                return StatusCode(200);
            }
        }

        
        [HttpPut("carrinho/atualizar/{login}")]
        public async Task<StatusCodeResult> AtualizarItemCarrinho(string login, CarrinhoItem itemCarrinho)
        {
            var cliente = await _context.Pessoas.FirstOrDefaultAsync(p => p.Login == login);
            var carrinho = await _context.CarrinhoCliente
                                      .Include(c => c.Itens).FirstOrDefaultAsync(x => x.ClienteId == cliente.Id);

            var itemExistente = carrinho.Itens.FirstOrDefault(c => c.ProdutoId == itemCarrinho.ProdutoId);
            if (itemExistente != null)
            {
                var itemNovo = itemExistente;
                carrinho.Itens.Remove(itemExistente);

                itemNovo.Quantidade = itemCarrinho.Quantidade;
                carrinho.Itens.Add(itemNovo);
            }

            decimal total = 0;

            foreach (CarrinhoItem item in carrinho.Itens)
            {
                total += item.Quantidade * item.Valor;
            }

            carrinho.ValorTotal = total;

            var itemDoCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProdutoId == itemExistente.ProdutoId);

            _context.CarrinhoItem.Update(itemDoCarrinho);
            _context.CarrinhoCliente.Update(carrinho);

            await _context.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpDelete("carrinho/remover-item/{login}&{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(string login, Guid produtoId)
        {
            var cliente = await _context.Pessoas.FirstOrDefaultAsync(p => p.Login == login);
            var carrinho = await _context.CarrinhoCliente
                                      .Include(c => c.Itens).FirstOrDefaultAsync(x => x.ClienteId == cliente.Id);

            var itemDoCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProdutoId == produtoId);
            carrinho.Itens.Remove(itemDoCarrinho);

            _context.CarrinhoItem.Remove(itemDoCarrinho);
            _context.CarrinhoCliente.Update(carrinho);

            await _context.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}