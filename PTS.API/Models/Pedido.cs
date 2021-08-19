using System;
using System.Collections.Generic;

namespace DBR.API.Models
{
    public class Pedido : Entity
	{
        public Pedido() { }

        public int Codigo { get;  set; }
        public Guid ClienteId { get;  set; }
        public decimal ValorTotal { get; set; }
        public DateTime Data { get;  set; }
        public int Status { get;  set; }
        
        public EnderecoPedido Endereco { get;  set; }

        public string NumeroCartao { get; set; }
        public string NomeCartao { get; set; }
        public string ExpiracaoCartao { get; set; }
        public string CvvCartao { get; set; }

        public List<PedidoItem> PedidoItems { get; set; } = new List<PedidoItem>();
    }
}
