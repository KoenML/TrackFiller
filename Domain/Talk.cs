using System;

namespace Domain
{
    public class Talk
    {
        public String Title { get; }
        public TimeSpan Duration { get; }

        public Talk(string title, TimeSpan duration)
        {
            Title = title;
            Duration = duration;
        }

        protected bool Equals(Talk other)
        {
            return string.Equals(Title, other.Title) && Duration.Equals(other.Duration);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Talk) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Title != null ? Title.GetHashCode() : 0) * 397) ^ Duration.GetHashCode();
            }
        }
    }
}