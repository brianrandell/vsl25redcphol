using AdvancedCopilotAPI.Data;
using AdvancedCopilotAPI.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace AdvancedCopilotAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        private IProductRepository? _productRepository;
        
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IProductRepository ProductRepository => 
            _productRepository ??= new ProductRepository(_context);
        
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }
        
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}