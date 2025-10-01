using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.produtos.AsNoTracking().ToList();

            if (!produtos.Any())
                return NotFound("Nenhum produto encontrado.");

            return produtos;
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")] //id:int:min(1)} restrição de rota no minimo valor maior que 1
        public ActionResult<Produto> ObterPorId(int id, string param2)
        {
            var parametro2 = param2;
            var produto = _context.produtos.AsNoTracking().FirstOrDefault(x => x.ProdutoId == id);

            if (produto is null)
                return NotFound("Produto não encontrado.");

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest("Produto inválido.");

            _context.produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest("Produto inválido.");

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.produtos.FirstOrDefault(x => x.ProdutoId == id);

            if (produto is null)
                return NotFound("Produto não encontrado.");

            _context.produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}