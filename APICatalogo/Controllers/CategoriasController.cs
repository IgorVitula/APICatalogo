using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(p=> p.Produtos).ToList();
        }


        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                var categorias = _context.Categorias.ToList();

                if (categorias is null || !categorias.Any())
                    return NotFound("Nenhuma categoria encontrada.");

                return categorias;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao obter categorias.");               
            }
          
        }


        [HttpGet]
        [Route("{id:int}", Name = "ObterCategoriaPorId")]
        public ActionResult<Categoria> GetId(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);

            if(categoria is null)
                return NotFound("Categoria não encontrada.");

            return categoria;
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria) {

            if(categoria is null)
                return BadRequest("Categoria inválida.");

            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return new CreatedAtRouteResult("ObterCategoriaPorId", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                    return BadRequest("Categoria inválida.");

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Ocorreu um erro ao atualizar a categoria.");
            }
           

        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete (int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);

            if (categoria == null)
                return BadRequest("Nenhuma categoria encontrada.");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
