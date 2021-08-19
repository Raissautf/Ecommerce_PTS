using System;
using System.Text.Json.Serialization;

namespace DBR.API.Models
{
    public class PedidoItem : Entity
	{
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string Imagem { get; set; }

        public Guid PedidoId { get; set; }
        // EF Rel.
        [JsonIgnore]
        public Pedido Pedido { get; set; }
    }
}
