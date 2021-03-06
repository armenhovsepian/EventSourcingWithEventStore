﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Entities;
using WebAPI.Services;
using static WebAPI.Events.ProductEvents;
using static WebAPI.Models.RequestModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IEventStoreService _eventStoreService;

        public ProductController(IProductService productService, IEventStoreService eventStoreService)
        {
            _productService = productService;
            _eventStoreService = eventStoreService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(CancellationToken ct)
        {
            var products = await _productService.GetAllListAsync(ct);
            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<Product>> Get(int id, CancellationToken ct)
        {
            var product = await _productService.GetByIdAsync(id, ct);
            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Product model, CancellationToken ct)
        {
            await _productService.AddAsync(model, ct);
            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
        }

        // PUT: api/product/name/5
        [Route("name/{id}")]
        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] V1.UpdateProductName model, CancellationToken ct)
        {
            if (id != model.Id) return BadRequest();

            var product = await _productService.GetByIdAsync(id, ct);
            if (product == null) return NotFound();

            product.UpdateName(model.Name);

            await _productService.UpdateAsync(product, ct);

            return NoContent();
        }

        // PUT: api/product/price/5
        [Route("price/{id}")]
        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] V1.UpdateProductPrice model, CancellationToken ct)
        {
            if (id != model.Id) return BadRequest();

            var product = await _productService.GetByIdAsync(id, ct);
            if (product == null) return NotFound();

            product.UpdatePrice(model.Price);

            await _productService.UpdateAsync(product, ct);

            return NoContent();
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, CancellationToken ct)
        {
            var product = await _productService.GetByIdAsync(id, ct);

            if (product == null) return NotFound();

            await _productService.DeleteAsync(product, ct);

            return NoContent();
        }



        // Get product change history
        [Route("history/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetEventsAsync(int id)
        {
            var product = await _eventStoreService.Load(id);

            var priceHistory = product.GetChanges()
                .Where(e => e is ProductPriceUpdated)
                .Select(e => (ProductPriceUpdated)e)
                .Select(e => new { e.Price, e.Created });

            return Ok(priceHistory);
        }

    }
}
