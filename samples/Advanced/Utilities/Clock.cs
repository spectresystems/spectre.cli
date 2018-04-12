using System;

namespace Advanced.Utilities
{
    public interface IClock
    {
        DateTime Now();
    }

    public sealed class Clock : IClock
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
