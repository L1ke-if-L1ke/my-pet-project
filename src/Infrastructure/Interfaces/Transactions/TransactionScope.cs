using Microsoft.EntityFrameworkCore.Storage;            
using Infrastructure.Persistence;                       
using UseCases.Interfaces;

namespace Infrastructure.Transactions
{
    public sealed class TransactionScope : ITransactionScope
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbContextTransaction _transaction;
        private bool _isDisposed;

        public TransactionScope(ApplicationDbContext context, IDbContextTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            await _transaction.CommitAsync(ct);
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            await _transaction.RollbackAsync(ct);
        }

        public async ValueTask DisposeAsync()
        {
            if (!_isDisposed)
            {
                await _transaction.DisposeAsync();
                _isDisposed = true;
            }
        }
    }
}
