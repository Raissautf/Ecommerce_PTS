using System;

namespace DBR.Application.Models
{
    public class EnderecoPedido
	{
        public Guid Id { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public Guid PessoaId { get; set; }

        public override string ToString()
        {
            return $"{Logradouro}, {Numero} {Complemento} - {Bairro} - {Cidade} - {Estado}";
        }

    }
}
