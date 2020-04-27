using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Entities;

namespace WebAPI.Services
{
    public interface IProductService
    {
        Task<Product> GetByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<Product>> GetAllListAsync(CancellationToken ct);
        Task AddAsync(Product entity, CancellationToken ct);
        Task UpdateName(Product entity, CancellationToken ct);
        Task UpdatePrice(Product entity, CancellationToken ct);

        Task DeleteAsync(Product entity, CancellationToken ct);
    }


    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductRepository _productRepository;

        public ProductService(AppDbContext dbContext, IProductRepository productRepository)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
        }

        public async Task<IReadOnlyList<Product>> GetAllListAsync(CancellationToken ct)
            => await _productRepository.GetAllListAsync(ct);


        public async Task<Product> GetByIdAsync(int id, CancellationToken ct)
            => await _productRepository.GetByIdAsync(id, ct);

        public async Task AddAsync(Product entity, CancellationToken ct)
        {
            await _productRepository.AddAsync(entity, ct);
            await SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Product entity, CancellationToken ct)
        {
            await _productRepository.DeleteAsync(entity, ct);
            await SaveChangesAsync(ct);
        }

        public async Task UpdateName(Product entity, CancellationToken ct)
        {
            await UpdateAsync(entity, ct);

        }

        public async Task UpdatePrice(Product entity, CancellationToken ct)
        {
            await UpdateAsync(entity, ct);
        }

        private async Task UpdateAsync(Product entity, CancellationToken ct)
        {
            await _productRepository.UpdateAsync(entity, ct);
            await SaveChangesAsync(ct);
        }

        private async Task SaveChangesAsync(CancellationToken ct)
            => await _dbContext.SaveChangesAsync(ct);


    }
}
