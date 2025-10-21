namespace AspireApp1.ServiceDefaults.SwapBook
{
    public record StartTimeBook
    {
        public DateOnly DateStart;
        public StartTimeBook(DateOnly dateStart)
        {
            DateStart = dateStart;
        }
    }
}
