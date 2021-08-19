using System.Collections.Generic;

namespace DBR.API.Models
{
	public class UsuarioRegistro
	{
		public string Nome { get; set; }
		public string Cpf { get; set; }
		public string Email { get; set; }
		public string Senha { get; set; }
		public string SenhaConfirmacao { get; set; }
	}

	public class UsuarioLogin
	{
		public string Email { get; set; }
		public string Senha { get; set; }
	}

	public class UsuarioRespostaLogin
	{
		public string AccessToken { get; set; }
		public double ExpiresIn { get; set; }
		public UsuarioToken UsuarioToken { get; set; }
	}

	public class UsuarioToken
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public IEnumerable<UsuarioClaim> Claims { get; set; }
	}

	public class UsuarioClaim
	{
		public string Value { get; set; }
		public string Type { get; set; }
	}
}
