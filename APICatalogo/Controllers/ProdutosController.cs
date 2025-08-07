using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
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
            var produtos = _context.produtos.ToList();
            if (produtos is null )            
                return NotFound();
            
            return produtos;
        }

        [HttpGet]
        [Route("{id:int}", Name ="ObterProduto")]
        public ActionResult<Produto> ObterPorId(int id)
        {
            var produto = _context.produtos.FirstOrDefault(x => x.ProdutoId == id);
            if (produto is null)            
                return NotFound("Produto Não encontrado");
            
            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if(produto is null)
            {
                return BadRequest("Produto inválido");
            }

            _context.produtos.Add(produto);
            _context.SaveChanges(); // persiste os dados na tabela 
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto); // aqui nesse retorno ele ja chama a rota de cima com o id do novo protudo pra vermos o retorno dele criado
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult put(int id, Produto produto) // put é atualização completa
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest("Produto inválido");
            }
            _context.Entry(produto).State = EntityState.Modified; // informa que o produto foi modificado
            _context.SaveChanges();
            return Ok(produto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.produtos.FirstOrDefault(x => x.ProdutoId == id);
            if (produto is null)
            {
                return BadRequest("Produto Não encontrado");
            }
            _context.produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
