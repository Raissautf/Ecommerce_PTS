using System;
using System.Text.Json.Serialization;

namespace DBR.API.Models
{
    public class CarrinhoItem : Entity
    {
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string Imagem { get; set; }

        public Guid CarrinhoClienteId { get; set; }

        [JsonIgnore]
        public CarrinhoCliente CarrinhoCliente { get; set; }
    }
}
