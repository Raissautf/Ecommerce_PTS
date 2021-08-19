using DBR.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBR.Application.Auxiliar
{
	public class CarrinhoViewComponent : ViewComponent
	{
		/* Componente de Carrinho, criado usado na master page para obter o carrinho do cliente*/
		private readonly IHttpContextAccessor _contextAcessor;
		private static readonly HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44370") };

		public CarrinhoViewComponent(IHttpContextAccessor contextAcessor)
		{
			_contextAcessor = contextAcessor;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			CarrinhoViewModel carrinho = new CarrinhoViewModel();
			try
			{
				var login = _contextAcessor.HttpContext.User.Identity.Name;

				var response = await _httpClient.GetAsync($"/carrinho/obtercarrinho/{login}");
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
				};

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
					carrinho = JsonSerializer.Deserialize<CarrinhoViewModel>(await response.Content.ReadAsStringAsync(), options);
			}
			catch(Exception ex)
			{
				var teste = "";
			}
			return View(carrinho);
		}
	}
}
