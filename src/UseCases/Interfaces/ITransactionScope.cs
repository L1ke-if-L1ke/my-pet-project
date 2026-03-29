namespace UseCases.Interfaces
{
    public interface ITransactionScope : IAsyncDisposable
    {
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }
}
