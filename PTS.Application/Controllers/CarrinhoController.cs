using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DBR.Application.Auxiliar;
using DBR.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DBR.Application.Controllers
{
    //Controller de Carrinho  para realizar o CRUD do carrinho de compras

    public class CarrinhoController : Controller
    {
        // HttpCliente é usado para fazer a comunicação com a API
        private readonly HttpClient _httpClient = new HttpClient();

        public CarrinhoController()
        {
            /*Endereço da API, esse endereço é configurado no projeto de API, então quando rodar no Browser
             irá subir na porta 44370
             */

            _httpClient.BaseAddress = new Uri("https://localhost:44370");
        }

        public async Task<IActionResult> Index()
        {
            var carrinho = await LeituraDados.ObterCarrinhoCliente(User.Identity.Name);

            return View(carrinho);
        }

        [HttpPost]
        [Route("carrinho/adicionar-item")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho)
        {
            var produto = await LeituraDados.ObterProdutoPorId(itemCarrinho.ProdutoId);

            //Adicionar Item ao carrinho
            itemCarrinho.Imagem = produto.Imagem;
            itemCarrinho.Nome = produto.Nome;
            itemCarrinho.Valor = produto.Valor;

            var jsonInString = JsonConvert.SerializeObject(itemCarrinho);
            var content = new StringContent(jsonInString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"/carrinho/adicionar/{User.Identity.Name}", content);

            //Redireciona para a ação Index desse controller 
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/atualizar-item")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
        {
            var produto = await LeituraDados.ObterProdutoPorId(produtoId);

            var itemCarrinho = new ItemCarrinhoViewModel { ProdutoId = produtoId, Quantidade = quantidade };

            var jsonInString = JsonConvert.SerializeObject(itemCarrinho);
            var content = new StringContent(jsonInString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/carrinho/atualizar/{User.Identity.Name}", content);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/remover-item")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/carrinho/remover-item/{User.Identity.Name}&{produtoId}");

            return RedirectToAction("Index");
        }

    }
}