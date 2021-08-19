using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DBR.Application.Models
{
    public class PedidoViewModel
    {
        public Guid Id { get; set; }
        public int Codigo { get; set; }

        public Guid ClienteId { get; set; }
        // Autorizado = 1,
        // Pago = 2,
        // Recusado = 3,
        // Entregue = 4,
        // Cancelado = 5
        public int Status { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }

        public List<ItemPedidoViewModel> PedidoItems { get; set; } = new List<ItemPedidoViewModel>();

        public EnderecoPedido Endereco { get; set; }

        [Required(ErrorMessage = "Informe o número do cartão")]
        [DisplayName("Número do Cartão")]
        public string NumeroCartao { get; set; }

        [Required(ErrorMessage = "Informe o nome do portador do cartão")]
        [DisplayName("Nome do Portador")]
        public string NomeCartao { get; set; }

        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "O vencimento deve estar no padrão MM/AA")]
        [Required(ErrorMessage = "Informe o vencimento")]
        [DisplayName("Data de Vencimento MM/AA")]
        public string ExpiracaoCartao { get; set; }

        [Required(ErrorMessage = "Informe o código de segurança")]
        [DisplayName("Código de Segurança")]
        public string CvvCartao { get; set; }

        [JsonIgnore]
        public string StatusString { get {
                if (Status == 1)
                    return "Autorizado";
                if (Status == 2)
                    return "Pago";

                if (Status == 3)
                    return "Recusado";

                if (Status == 4)
                    return "Entregue";

                if (Status == 5)
                    return "Cancelado";

                return "";
            }
        }

    }

    public class ItemPedidoViewModel
    {
        public Guid PedidoId { get; set; }
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string Imagem { get; set; }

        [JsonIgnore]
        public decimal ValorTotal { get { return Valor * Quantidade; }  }

    }
}
