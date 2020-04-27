using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Product
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _dbContext.Products.ToList();               
            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Product> Get(int id)
        {
            var product = _dbContext.Products.Find(id);
            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public ActionResult Post([FromBody] Product model)
        {
            _dbContext.Products.Add(model);
            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
        }

        // PUT: api/name/5
        [Route("name")]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string name)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null) return NotFound();

            product.Name = name;
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();

            return NoContent();
        }

        // PUT: api/price/5
        [Route("price")]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] decimal price)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null) return NotFound();

            product.Price = price;
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();

            return NoContent();
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null) return NotFound();

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
