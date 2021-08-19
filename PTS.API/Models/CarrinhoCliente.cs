using System;
using System.Collections.Generic;

namespace DBR.API.Models
{
	public class CarrinhoCliente : Entity
	{
		public Guid ClienteId { get; set; }
		public decimal ValorTotal { get; set; }
		public List<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();
	}
}
