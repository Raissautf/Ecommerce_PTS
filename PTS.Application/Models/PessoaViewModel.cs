using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBR.Application.Models
{
	public class PessoaViewModel
	{
		public Guid Id { get; set; }
		public string Nome { get; set; }
		public string Login { get; set; }

		public string Documento { get; set; }

		public string Email { get; set; }

		public string Senha { get; set; }

		public string SenhaConfirmacao { get; set; }
	}
}
