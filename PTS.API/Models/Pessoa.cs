namespace DBR.API.Models
{
	public class Pessoa : Entity
	{
		public string Nome { get; set; }
		public string Login { get; set; }
		public string Documento { get; set; }
		public string Email { get; set; }
		public string Senha { get; set; }
		public string SenhaConfirmacao { get; set; }
	}
}
