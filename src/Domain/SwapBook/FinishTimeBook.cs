namespace Domain.SwapBook
{
    public record FinishTimeBook
    {
        public DateOnly DateFinish;
        public FinishTimeBook(DateOnly dateFinish)
        {
            DateFinish = dateFinish;
        }
    }
}
