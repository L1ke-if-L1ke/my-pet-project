namespace UseCases.Interfaces
{
    public interface ITransactionFactory
    {
        Task<ITransactionScope> CreateAsync(CancellationToken ct = default);
    }
}
