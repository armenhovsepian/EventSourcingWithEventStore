using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Data;
using WebAPI.Entities;
using static WebAPI.Models.RequestModels;

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

        // PUT: api/product/name/5
        [Route("name/{id}")]
        [HttpPut]
        public ActionResult Put(int id, [FromBody] V1.UpdateProductName model)
        {
            if (id != model.Id) return BadRequest();

            var product = _dbContext.Products.Find(id);
            if (product == null) return NotFound();

            product.Name = model.Name;
            product.Modified = DateTime.Now;
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();

            return NoContent();
        }

        // PUT: api/product/price/5
        [Route("price/{id}")]
        [HttpPut]
        public ActionResult Put(int id, [FromBody] V1.UpdateProductPrice model)
        {
            if (id != model.Id) return BadRequest();

            var product = _dbContext.Products.Find(id);
            if (product == null) return NotFound();

            product.Price = model.Price;
            product.Modified = DateTime.Now;
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
