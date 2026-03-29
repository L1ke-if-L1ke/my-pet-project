using Microsoft.EntityFrameworkCore;                    
using Infrastructure.Persistence;                       
using UseCases.Interfaces;

namespace Infrastructure.Transactions
{
    public sealed class TransactionFactory : ITransactionFactory
    {
        private readonly ApplicationDbContext _context;

        public TransactionFactory(ApplicationDbContext context) => _context = context;

        public async Task<ITransactionScope> CreateAsync(CancellationToken ct = default)
        {
            var transaction = await _context.Database.BeginTransactionAsync(ct);
            return new TransactionScope(_context, transaction);
        }
    }
}
