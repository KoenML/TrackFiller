using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public class Track
    {
        public List<Talk> AMTalks { get; set; }
        public List<Talk> PMTalks { get; set; }

        public Track()
        {
            AMTalks = new List<Talk>();
            PMTalks = new List<Talk>();
        }

        protected bool Equals(Track other)
        {
            return Equals(AMTalks, other.AMTalks) && Equals(PMTalks, other.PMTalks);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Track) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((AMTalks != null ? AMTalks.GetHashCode() : 0) * 397) ^ (PMTalks != null ? PMTalks.GetHashCode() : 0);
            }
        }
    }
}