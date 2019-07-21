namespace BL.Exceptions
{
    public class NotEnoughTalksException : TimeRestraintException
    {
        public double MinutesShort { get; }

        public NotEnoughTalksException(double minutesShort)
        {
            MinutesShort = minutesShort;
        }
    }
}