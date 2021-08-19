using DBR.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DBR.API.Data
{
	public class ApiContext : DbContext
	{
		public ApiContext(DbContextOptions<ApiContext> options)
			: base(options) { }


		//Entidades que vão simular as Tabelas de banco de dados, EntityFrameworkCore
		public DbSet<Produto> Produtos { get; set; }
		public DbSet<Pessoa> Pessoas { get; set; }
		public DbSet<CarrinhoCliente> CarrinhoCliente { get; set; }
		public DbSet<CarrinhoItem> CarrinhoItem { get; set; }
		public DbSet<Endereco> Endereco { get; set; }
		public DbSet<Pedido> Pedido { get; set; }
		public DbSet<PedidoItem> PedidoItem { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
					e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
			{
				property.SetColumnType("varchar(100)");
			}

			/*Configurações do entityframeworkCore para relacionamento entre tabelas*/

			modelBuilder.Entity<CarrinhoCliente>()
					.HasIndex(c => c.ClienteId)
					.HasName("IDX_Cliente");

			modelBuilder.Entity<CarrinhoCliente>()
				.HasMany(c => c.Itens)
				.WithOne(i => i.CarrinhoCliente)
				.HasForeignKey(c => c.CarrinhoClienteId);

			modelBuilder.Entity<Pedido>()
				.HasIndex(c => c.ClienteId)
				.HasName("IDX_Pedido_Cliente");

			modelBuilder.Entity<Pedido>()
				.HasMany(c => c.PedidoItems)
				.WithOne(i => i.Pedido)
				.HasForeignKey(c => c.PedidoId);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiContext).Assembly);
		}
	}
}
