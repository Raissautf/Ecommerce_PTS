using DBR.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBR.Application.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public CatalogoController()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44370");
        }

        public async Task<IActionResult> Index()
        {
            //Realiza a busca dos produtos na API de catálogo
            var response = await _httpClient.GetAsync("/catalogo/produtos/");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            //Deserializa os dados que vem da API em formato JSON para a calasse ProdutoViewModel
            var dados = JsonSerializer.Deserialize<IEnumerable<ProdutoViewModel>>(await response.Content.ReadAsStringAsync(), options);

            return View(dados);
        }

        [HttpGet]
        //[Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var produto = JsonSerializer.Deserialize<ProdutoViewModel>(await response.Content.ReadAsStringAsync(), options);

            return View(produto);
        }

    }
}