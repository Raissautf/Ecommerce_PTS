using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DBR.Application.Models
{
	public class ItemCarrinhoViewModel
	{
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string Imagem { get; set; }

        [NotMapped]
        [JsonIgnore]
        public decimal TotalItem { 
            get { return Valor * Quantidade; }  }

        public Guid CarrinhoId { get; set; }
        public CarrinhoViewModel CarrinhoCliente { get; set; }
    }
}
