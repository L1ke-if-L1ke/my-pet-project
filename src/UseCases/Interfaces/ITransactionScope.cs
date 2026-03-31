namespace UseCases.Interfaces
{
    public interface ITransactionScope : IAsyncDisposable, IDisposable
    {
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }
}
