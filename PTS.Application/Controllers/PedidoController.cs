using DBR.Application.Auxiliar;
using DBR.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Application.Controllers
{
	public class PedidoController : Controller
	{
		private readonly HttpClient _httpClient = new HttpClient();

		public PedidoController()
		{
			_httpClient.BaseAddress = new Uri("https://localhost:44370");
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		[Route("endereco-de-entrega")]
		public async Task<IActionResult> EnderecoEntrega()
		{
			var carrinho = await LeituraDados.ObterCarrinhoCliente(User.Identity.Name);

			if (carrinho.Itens.Count == 0)
				return RedirectToAction("Index", "Carrinho");

			var endereco = await LeituraDados.ObterEndereco(User.Identity.Name);

			var pedido = new PedidoViewModel();
			pedido.ValorTotal = carrinho.ValorTotal;
			ItemPedidoViewModel itemPedidoViewModel;

			foreach (var item in carrinho.Itens)
			{
				itemPedidoViewModel = new ItemPedidoViewModel();
				itemPedidoViewModel.Imagem = item.Imagem;
				itemPedidoViewModel.Nome = item.Nome;
				itemPedidoViewModel.ProdutoId = item.ProdutoId;
				itemPedidoViewModel.Quantidade = item.Quantidade;
				itemPedidoViewModel.Valor = item.Valor;
				pedido.PedidoItems.Add(itemPedidoViewModel);
			}

			if (endereco != null)
			{
				pedido.Endereco = new EnderecoPedido
				{
					Logradouro = endereco.Logradouro,
					Numero = endereco.Numero,
					Bairro = endereco.Bairro,
					Cep = endereco.Cep,
					Complemento = endereco.Complemento,
					Cidade = endereco.Cidade,
					Estado = endereco.Estado
				};
			}

			return View(pedido);
		}


		[HttpGet]
		[Route("pagamento")]
		public async Task<IActionResult> Pagamento()
		{
			var carrinho = await LeituraDados.ObterCarrinhoCliente(User.Identity.Name);
			if (carrinho.Itens.Count == 0) return RedirectToAction("Index", "Carrinho");

			var pedido = new PedidoViewModel();
			pedido.ValorTotal = carrinho.ValorTotal;
			ItemPedidoViewModel itemPedidoViewModel;

			foreach (var item in carrinho.Itens)
			{
				itemPedidoViewModel = new ItemPedidoViewModel();
				itemPedidoViewModel.Imagem = item.Imagem;
				itemPedidoViewModel.Nome = item.Nome;
				itemPedidoViewModel.ProdutoId = item.ProdutoId;
				itemPedidoViewModel.Quantidade = item.Quantidade;
				itemPedidoViewModel.Valor = item.Valor;
				pedido.PedidoItems.Add(itemPedidoViewModel);
			}

			var endereco = await LeituraDados.ObterEndereco(User.Identity.Name);

			if (endereco != null)
			{
				pedido.Endereco = new EnderecoPedido
				{
					Logradouro = endereco.Logradouro,
					Numero = endereco.Numero,
					Bairro = endereco.Bairro,
					Cep = endereco.Cep,
					Complemento = endereco.Complemento,
					Cidade = endereco.Cidade,
					Estado = endereco.Estado
				};
			}

			return View(pedido);
		}


		[HttpPost]
		[Route("finalizar-pedido")]
		public async Task<IActionResult> FinalizarPedido(PedidoViewModel pedidoViewModel)
		{
			if (ModelState.IsValid)
			{
				var carrinho = await LeituraDados.ObterCarrinhoCliente(User.Identity.Name);

				var pedido = new PedidoViewModel();
				pedido.ValorTotal = carrinho.ValorTotal;

				foreach (var item in carrinho.Itens)
				{
					var pedidoItem = new ItemPedidoViewModel();

					pedidoItem.Valor = item.Valor;
					pedidoItem.Id = item.Id;
					pedidoItem.Imagem = item.Imagem;
					pedidoItem.Nome = item.Nome;
					pedidoItem.Quantidade = item.Quantidade;
					pedidoItem.ProdutoId = item.ProdutoId;

					pedidoViewModel.PedidoItems.Add(pedidoItem);
				}

				var endereco = await LeituraDados.ObterEndereco(User.Identity.Name);

				if (endereco != null)
				{
					pedidoViewModel.Endereco = new EnderecoPedido
					{
						Logradouro = endereco.Logradouro,
						Numero = endereco.Numero,
						Bairro = endereco.Bairro,
						Cep = endereco.Cep,
						Complemento = endereco.Complemento,
						Cidade = endereco.Cidade,
						Estado = endereco.Estado
					};
				}

				var jsonInString = JsonConvert.SerializeObject(pedidoViewModel);
				var content = new StringContent(jsonInString, Encoding.UTF8, "application/json");

				var response = await _httpClient.PostAsync($"compras/pedido/{User.Identity.Name}", content);
			}

			return RedirectToAction("PedidoConcluido");
		}
		
		
		[HttpGet]
		[Route("pedido-concluido")]
		public async Task<IActionResult> PedidoConcluido()
		{
			var pedido = await LeituraDados.ObterUltimoPedidoCliente(User.Identity.Name);

			return View("ConfirmacaoPedido", pedido);
		}

		[HttpGet("meus-pedidos")]
		public async Task<IActionResult> MeusPedidos()
		{
			var pedidos = await LeituraDados.ObterListaPedidoCliente(User.Identity.Name);

			return View(pedidos);
		}
		
	}
}