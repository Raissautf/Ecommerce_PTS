using DBR.Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Application.Controllers
{
	public class IdentidadeController : Controller
	{
		private readonly HttpClient _httpClient = new HttpClient();
		
		public IdentidadeController()
		{
			_httpClient.BaseAddress = new Uri("https://localhost:44370");
		}

		[HttpGet]
		[Route("nova-conta")]
		public IActionResult Registro()
		{
			return View();
		}

		[HttpGet]
		[Route("login")]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[Route("nova-conta")]
		public async Task<ActionResult> Registro(PessoaViewModel pessoa)
		{
			if (ModelState.IsValid)
			{
				
				/* Parar inserir dados no banco usando a API, é
				 * preciso obter os dados que vem da tela, preencher uma classe, formatar os dados para JSON depois chamar a  API passando
				 o arquivo JSON como parâmetro*/
				var jsonInString = JsonConvert.SerializeObject(pessoa);
				var content = new StringContent(jsonInString, Encoding.UTF8, "application/json");
				var response = await _httpClient.PostAsync("/cliente/adicionar", content);

				if (response.StatusCode == HttpStatusCode.OK)
					return RedirectToAction("Login", pessoa);
			}

			return View();
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(PessoaViewModel pessoa)
		{
			ModelState.Remove("Nome");
			ModelState.Remove("Email");

			if (ModelState.IsValid)
			{
				var jsonInString = JsonConvert.SerializeObject(pessoa);
				var content = new StringContent(jsonInString, Encoding.UTF8, "application/json");
				var response = await _httpClient.PostAsync("/cliente/valida_login", content);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, pessoa.Login)
					};

					ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
					ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

					//Método que faz a autenticação no Browser
					await HttpContext.SignInAsync(principal);

					return RedirectToAction("ValidarLogin", "Identidade");
				}
				else
				{
					return View();
				}
			}
			else
			{
				return View();
			}
		}

		[HttpGet]
		public IActionResult ValidarLogin()
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Catalogo");
			else
				return RedirectToAction("Login");
		}

		[HttpGet("logout")]
		public async Task<IActionResult> Logout()
		{
			//Método que faz o logout no Browser
			await HttpContext.SignOutAsync();

			return RedirectToAction("ValidarLogin", "Identidade");
		}
		[HttpPost]
		public async Task<IActionResult> NovoEndereco(EnderecoViewModel endereco)
		{
			try
			{
				var jsonInString = JsonConvert.SerializeObject(endereco);
				var content = new StringContent(jsonInString, Encoding.UTF8, "application/json");
				var response = await _httpClient.PostAsync($"/cliente/endereco/{User.Identity.Name}", content);

			}
			catch (Exception ex)
			{

			}
			return RedirectToAction("EnderecoEntrega", "Pedido");
		}

	}
}