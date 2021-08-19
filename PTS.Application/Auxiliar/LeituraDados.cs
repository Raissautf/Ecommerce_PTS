using DBR.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBR.Application.Auxiliar
{
	public class LeituraDados : Controller
	{
		/*Classe criada para realizar de dados na API */

		private static readonly HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44370") };
		HttpContextAccessor _contextAcessor;

		public LeituraDados(HttpContextAccessor acessor)
		{
			_contextAcessor = acessor;
		}

		public static async Task<ProdutoViewModel> ObterProdutoPorId(Guid idProduto)
		{
			var response = await _httpClient.GetAsync($"/catalogo/produtos/{idProduto}");

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};

			var dados = JsonSerializer.Deserialize<ProdutoViewModel>(await response.Content.ReadAsStringAsync(), options);

			return dados;
		}

		public static async Task<CarrinhoViewModel> ObterCarrinhoCliente(string login)
		{
			var clienteLogin = login;

			var response = await _httpClient.GetAsync($"/carrinho/obtercarrinho/{clienteLogin}");
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};

			var carrinho = JsonSerializer.Deserialize<CarrinhoViewModel>(await response.Content.ReadAsStringAsync(), options);

			return carrinho;
		}

		public static async Task<PedidoViewModel> ObterUltimoPedidoCliente(string login)
		{
			var clienteLogin = login;

			var response = await _httpClient.GetAsync($"/pedido/ultimo/{clienteLogin}");
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};

			var pedido = JsonSerializer.Deserialize<PedidoViewModel>(await response.Content.ReadAsStringAsync(), options);

			return pedido;
		}

		public static async Task<List<PedidoViewModel>> ObterListaPedidoCliente(string login)
		{
			var clienteLogin = login;

			var response = await _httpClient.GetAsync($"/pedido/lista-cliente/{clienteLogin}");
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};

			var pedidos = JsonSerializer.Deserialize<List<PedidoViewModel>>(await response.Content.ReadAsStringAsync(), options);

			return pedidos;
		}

		public static async Task<EnderecoViewModel> ObterEndereco(string login)
		{
			var response = await _httpClient.GetAsync($"/cliente/endereco/{login}");
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};

			if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
				return null;

			var endereco = JsonSerializer.Deserialize<EnderecoViewModel>(await response.Content.ReadAsStringAsync(), options);

			return endereco;
		}

	}
}
