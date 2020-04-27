using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Entities;

namespace WebAPI.Data
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<Product>> GetAllListAsync(CancellationToken ct);
        Task AddAsync(Product entity, CancellationToken ct);
        Task UpdateAsync(Product entity, CancellationToken ct);
        Task DeleteAsync(Product entity, CancellationToken ct);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
            => _dbContext = dbContext;


        public async Task AddAsync(Product entity, CancellationToken ct)
            => await _dbContext.Products.AddAsync(entity, ct);

        public async Task DeleteAsync(Product entity, CancellationToken ct)
        {
            _dbContext.Products.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IReadOnlyList<Product>> GetAllListAsync(CancellationToken ct)
           => await _dbContext.Products.ToListAsync(ct);

        public async Task<Product> GetByIdAsync(int id, CancellationToken ct)
           => await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == id, ct);

        public async Task UpdateAsync(Product entity, CancellationToken ct)
        {
            _dbContext.Products.Update(entity);
            await Task.CompletedTask;
        }
    }

}
