using System;
using System.Collections;
using System.ComponentModel.Design;
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

        public Track BuildTrack(ArrayList talks, out ArrayList remainingTalks)
        {
           Track result = new Track();
           result.AMTalks = AMBuilder(talks, out remainingTalks);
           result.PMTalks = PMBuilder(remainingTalks, out remainingTalks);

           return result;
        }

        private ArrayList AMBuilder(ArrayList talks, out ArrayList remainingTalks)
        {
            TimeSpan amDuration = TimeSpan.Zero;
            ArrayList result = new ArrayList();
            while (TimeSpan.Compare(amDuration, TimeSpan.FromHours(AmHours)) != 0)
            {
                Talk query = talks.Cast<Talk>()
                    .Where(talk => ((talk.Duration + amDuration).TotalHours <= AmHours))
                    .OrderByDescending(talk => talk.Duration)
                    .Select(talk => talk)
                    .First();

                amDuration += query.Duration;
                result.Add(query);
                talks.Remove(query);
            }

            remainingTalks = talks;
            return result;
        }

        private ArrayList PMBuilder(ArrayList talks, out ArrayList remainingTalks)
        {
            TimeSpan pmDuration = TimeSpan.Zero;
            ArrayList result = new ArrayList();
            while (TimeSpan.Compare(pmDuration, TimeSpan.FromHours(PmHoursMin)) == 1 && TimeSpan.Compare(pmDuration, TimeSpan.FromHours(PmHoursMax)) == -1)
            {
                Talk query = talks.Cast<Talk>()
                    .Where(talk => (talk.Duration + pmDuration).TotalHours <= PmHoursMax && (talk.Duration + pmDuration).TotalHours >= PmHoursMin)
                    .OrderByDescending(talk => talk.Duration)
                    .Select(talk => talk)
                    .First();

                pmDuration += query.Duration;
                result.Add(query);
                talks.Remove(query);
            }

            remainingTalks = talks;
            return result;
        }
    }
}