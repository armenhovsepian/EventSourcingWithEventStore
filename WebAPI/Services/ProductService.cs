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
        Task DeleteAsync(Product entity, CancellationToken ct);
        Task UpdateAsync(Product entity, CancellationToken ct);
    }


    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductRepository _productRepository;
        private readonly IEventStoreService _eventStoreService;

        public ProductService(AppDbContext dbContext, IProductRepository productRepository,
            IEventStoreService eventStoreService)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
            _eventStoreService = eventStoreService;
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

        public async Task UpdateAsync(Product entity, CancellationToken ct)
        {
            await _productRepository.UpdateAsync(entity, ct);
            await SaveChangesAsync(ct);
            await _eventStoreService.Save(entity);
        }

        private async Task SaveChangesAsync(CancellationToken ct)
            => await _dbContext.SaveChangesAsync(ct);


    }
}
