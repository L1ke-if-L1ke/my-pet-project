using Microsoft.EntityFrameworkCore.Storage;            
using Infrastructure.Persistence;                       
using UseCases.Interfaces;

namespace Infrastructure.Transactions
{
    public sealed class TransactionScope : ITransactionScope
    {
        private readonly IDbContextTransaction _transaction;
        private bool _isDisposed;

        public TransactionScope(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            try
            {
                await _transaction.CommitAsync(ct);
            }
            finally
            {
                await DisposeAsync(); // Гарантированно освобождаем ресурсы, даже если коммит упал
            }
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            try
            {
                await _transaction.RollbackAsync(ct);
            }
            finally
            {
                await DisposeAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!_isDisposed)
            {
                await _transaction.DisposeAsync();
                _isDisposed = true;
            }
        }
        public void Dispose()  // Синхронный Dispose для совместимости
        {
            if (!_isDisposed)
            {
                _transaction.Dispose();  // Синхронная версия
                _isDisposed = true;
            }
        }
    }
}
