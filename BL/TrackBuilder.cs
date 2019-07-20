using System;
using System.Collections;
using System.Linq;
using Domain;

namespace BL
{
    public class TrackBuilder
    {
        public int AmHours { get; }
        public int PmHoursMin { get; }
        public int PmHoursMax { get; }

        public TrackBuilder(int amHours, int pmHoursMin, int pmHoursMax)
        {
            this.AmHours = amHours;
            this.PmHoursMin = pmHoursMin;
            this.PmHoursMax = pmHoursMax;
        }

        public (Track track, ArrayList remainingTalks) BuildTrack(ArrayList talks)
        {
           Track result = new Track();
           var amResult = AMBuilder(talks);
           result.AMTalks = amResult.amTalks;
           var pmResult = PMBuilder(amResult.remainingTalks);
           result.PMTalks = pmResult.pmTalks;
           return (track: result, remainingTalks: pmResult.remainingTalks);
        }

        private (ArrayList amTalks, ArrayList remainingTalks) AMBuilder(ArrayList talks)
        {
            TimeSpan amDuration = TimeSpan.Zero;
            ArrayList result = new ArrayList();
            ArrayList remainingTalks = new ArrayList();
            while (TimeSpan.Compare(amDuration, TimeSpan.FromHours(AmHours)) != 0 && 
                   talks.Cast<Talk>()
                       .Where(talk => ((talk.Duration + amDuration).TotalHours <= AmHours))
                       .OrderByDescending(talk => talk.Duration)
                       .Select(talk => talk).Any())
            {
                
                var query = talks.Cast<Talk>()
                    .Where(talk => ((talk.Duration + amDuration).TotalHours <= AmHours))
                    .OrderByDescending(talk => talk.Duration)
                    .Select(talk => talk)
                    .First();

                amDuration += query.Duration;
                result.Add(query);
                talks.Remove(query);
            }
            remainingTalks = talks;
            return (amTalks: result, remainingTalks: remainingTalks);
        }

        private (ArrayList pmTalks, ArrayList remainingTalks) PMBuilder(ArrayList talks)
        {
            TimeSpan pmDuration = TimeSpan.Zero;
            ArrayList result = new ArrayList();
            while ((TimeSpan.Compare(pmDuration, TimeSpan.FromHours(PmHoursMin)) <= 0 || TimeSpan.Compare(pmDuration, TimeSpan.FromHours(PmHoursMax)) < 0) &&
                   talks.Cast<Talk>()
                       .Where(talk => (talk.Duration + pmDuration).TotalHours <= PmHoursMax)
                       .OrderByDescending(talk => talk.Duration)
                       .Select(talk => talk)
                       .Any())
            {
                Talk query = talks.Cast<Talk>()
                    .Where(talk => (talk.Duration + pmDuration).TotalHours <= PmHoursMax)
                    .OrderByDescending(talk => talk.Duration)
                    .Select(talk => talk)
                    .First();

                pmDuration += query.Duration;
                result.Add(query);
                talks.Remove(query);
            }

            ArrayList remainingTalks = talks;
            return (pmTalks: result, remainingTalks: remainingTalks);
        }
    }
}