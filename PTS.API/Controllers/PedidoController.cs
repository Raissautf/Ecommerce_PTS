using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBR.API.Data;
using DBR.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBR.API.Controllers
{
    public class PedidoController : Controller
    {

        private readonly ApiContext _context;
        public PedidoController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("compras/pedido/{login}")]
        public async Task<StatusCodeResult> AdicionarPedido(string login, [FromBody]Pedido pedido)
        {
            try
            {
                var carrinho = await ObterCarrinho(login);

                pedido.ClienteId = carrinho.ClienteId;

                
                var listaPedidos = await _context.Pedido.Where(p => p.ClienteId == carrinho.ClienteId).ToListAsync();
                int codigo = 0;

                if (listaPedidos != null && listaPedidos.Count > 0)
                    codigo = listaPedidos.Last().Codigo + 1;
                else
                    codigo = 1;

                pedido.Status = 1;
                pedido.Codigo = codigo;

                pedido.ValorTotal = carrinho.ValorTotal;

                _context.Pedido.Add(pedido);
                var qtd = await _context.SaveChangesAsync();
                if (qtd > 0)
                {             
                    _context.CarrinhoCliente.Remove(carrinho);
                    int qtdCarrinhoExcluido = await _context.SaveChangesAsync();

                    return StatusCode(200);
                }
                else
                    return StatusCode(400);

            }
            catch(Exception ex) 
            {
                string teste = "";
            }

            return StatusCode(200);
        }

        private async Task<CarrinhoCliente> ObterCarrinho(string login)
        {
            var cliente = await _context.Pessoas.FirstOrDefaultAsync(p => p.Login == login);

            var carrinho = await _context.CarrinhoCliente.FirstAsync(x => x.ClienteId == cliente.Id);

            return carrinho;
        }

        [HttpPost("pedido/{login}")]
        public async Task<IActionResult> FinalizarPedido(string login, [FromBody]Pedido pedido)
        {
            byte[] buffer = new byte[785];

            var cliente = await _context.Pessoas.FirstOrDefaultAsync(p => p.Login == login);

            var carrinho = await _context.CarrinhoCliente.FirstAsync(x => x.ClienteId == cliente.Id);

            pedido.ClienteId = cliente.Id;

            _context.Pedido.Update(pedido);
            var qtd = await _context.SaveChangesAsync();
            if (qtd > 0)
            {

                return StatusCode(200);
            }
            else
                return StatusCode(400);
        }
        

        [HttpGet("pedido/ultimo/{login}")]
        public async Task<Pedido> UltimoPedido(string login)
        {
            var cliente = await _context.Pessoas.FirstOrDefaultAsync(p => p.Login == login);

            var pedidos = await _context.Pedido.Include(p => p.PedidoItems)
                                    .Where(p => p.ClienteId == cliente.Id).ToListAsync();

            Pedido ultimoPedido = new Pedido();

            if (pedidos != null && pedidos.Count > 0)
                ultimoPedido =  pedidos.Last();

            var endereco = await _context.Endereco.FirstOrDefaultAsync(e => e.PessoaId == cliente.Id);

            EnderecoPedido enderecoPedido = new EnderecoPedido();
            if (endereco != null)
            {
                enderecoPedido = new EnderecoPedido();
                enderecoPedido.Id = endereco.Id;
                enderecoPedido.Logradouro = endereco.Logradouro;
                enderecoPedido.Numero = endereco.Numero;
                enderecoPedido.PessoaId = endereco.PessoaId;
                enderecoPedido.Bairro = endereco.Bairro;
                enderecoPedido.Cidade = endereco.Cidade;
                enderecoPedido.Cep = endereco.Cep;
            }

            ultimoPedido.Endereco = enderecoPedido;

            return ultimoPedido;
        }
        
        [HttpGet("pedido/lista-cliente/{login}")]
        public async Task<List<Pedido>> ListaPorCliente(string login)
        {
            var cliente = await _context.Pessoas.FirstOrDefaultAsync(p => p.Login == login);

            var pedidos = await _context.Pedido.Include(p => p.PedidoItems)
                                    .Where(p => p.ClienteId == cliente.Id).ToListAsync();

            var endereco = await _context.Endereco.FirstOrDefaultAsync(e => e.PessoaId == cliente.Id);

            EnderecoPedido enderecoPedido = new EnderecoPedido();
            if (endereco != null)
            {
                enderecoPedido = new EnderecoPedido();
                enderecoPedido.Id = endereco.Id;
                enderecoPedido.Logradouro = endereco.Logradouro;
                enderecoPedido.Numero = endereco.Numero;
                enderecoPedido.PessoaId = endereco.PessoaId;
                enderecoPedido.Bairro = endereco.Bairro;
                enderecoPedido.Cidade = endereco.Cidade;
                enderecoPedido.Cep = endereco.Cep;
            }

            if (pedidos != null && pedidos.Count > 0)
            {
                foreach (var item in pedidos)
                {
                    item.Endereco = enderecoPedido;
                }
            }
                
            return pedidos;
        }

    }
}