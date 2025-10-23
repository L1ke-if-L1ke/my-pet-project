
namespace Domain.SwapBook
{
    public class SwapBook
    {
        public BookId BookId { get; }
        public StartTimeBook StartTime { get; private set; }
        public FinishTimeBook FinishTime { get; private set; }
        public UserId UserId { get; }
        public SwapBook(BookId bookId, StartTimeBook startTime, FinishTimeBook finishTime, UserId userId)
        {
            BookId = bookId;
            StartTime = startTime;
            FinishTime = finishTime;
            UserId = userId;
        }
    }
}
