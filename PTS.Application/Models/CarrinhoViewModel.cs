using System;
using System.Collections.Generic;

namespace DBR.Application.Models
{
	public class CarrinhoViewModel
	{
		public Guid Id { get; set; }
		public Guid ClienteId { get; set; }
		public decimal ValorTotal { get; set; }
		public List<ItemCarrinhoViewModel> Itens { get; set; } = new List<ItemCarrinhoViewModel>();
	}
}
