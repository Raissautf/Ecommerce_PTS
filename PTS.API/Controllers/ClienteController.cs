using DBR.API.Data;
using DBR.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DBR.API.Controllers
{
    [ApiController]
    public class ClienteController : Controller
    {
        private readonly ApiContext _context;

        public ClienteController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost("cliente/adicionar")]
        public StatusCodeResult CadastrarCliente(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            int qtd = _context.SaveChanges();

            if (qtd > 0)
                return StatusCode(200);

            return StatusCode(400);
        }

        [HttpPost("cliente/valida_login")]
        public StatusCodeResult ValidaLogin(Pessoa pessoa)
        {
            var login = _context.Pessoas.FirstOrDefault(p => p.Login == pessoa.Login && p.Senha == pessoa.Senha);

            if (login != null)
                return StatusCode(200);

            return StatusCode(400);
        }

        [HttpGet("cliente/obter_cliente/{email}")]
        public async Task<Pessoa> ObterCliente(string email)
        {
            var pessoa =  await _context.Pessoas.FindAsync(email);

            if (pessoa != null)
                return pessoa;

            return new Pessoa();
        }


        [HttpGet("cliente/obter_cliente_por_email/{email}")]
        public async Task<Pessoa> ObterClientePorEmail(string email)
        {
            var pessoa = await _context.Pessoas.FindAsync(email);

            if (pessoa != null)
                return pessoa;

            return new Pessoa();
        }


        [HttpGet("cliente/endereco/{login}")]
        public async Task<IActionResult> ObterEndereco(string login)
        {
            var pessoa = _context.Pessoas.FirstOrDefault(p => p.Login == login);
            var endereco = _context.Endereco.FirstOrDefault(e => e.PessoaId == pessoa.Id);
            
            if (endereco == null)
                return NotFound();
            else
                return Ok(endereco);
        }

        [HttpPost("cliente/endereco/{login}")]
        public StatusCodeResult AdicionarEndereco(string login, Endereco endereco)
        {
            var pessoa = _context.Pessoas.FirstOrDefault(p => p.Login == login);
            endereco.PessoaId = pessoa.Id;

            _context.Endereco.Add(endereco);
            int qtd = _context.SaveChanges();

            if (qtd > 0)
                return StatusCode(200);

            return StatusCode(400);
        }

    }
}