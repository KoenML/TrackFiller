using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BL.Exceptions;
using Domain;
using MoreLinq;

namespace BL.Builders
{
    public class SimulTrackBuilder : ITrackBuilder
    {
        
        public int AmHours { get; }
        public int PmHoursMin { get; }
        public int PmHoursMax { get; }

        public SimulTrackBuilder(int amHours, int pmHoursMin, int pmHoursMax)
        {
            AmHours = amHours;
            PmHoursMin = pmHoursMin;
            PmHoursMax = pmHoursMax;
        }


        public (List<Track> tracks, List<Talk> remainingTalks) BuildTracks(List<Talk> talks, int numberOfTracks)
        {
            //Check if number of tracks is possible with given talks and time constraints
            int timeNeeded = (AmHours + PmHoursMin) * numberOfTracks * 60;
            double totalTalkTime = talks.Cast<Talk>().Sum(talk => talk.Duration.TotalMinutes);
            if(totalTalkTime < timeNeeded){throw new NotEnoughTalksException(timeNeeded - totalTalkTime);}

            //Setup number of requested tracks
            List<Track> tracks = new List<Track>();
            for (int i = 0; i < numberOfTracks; i++)
            {
                Track t = new Track();
                tracks.Add(t);
            }

            //first fill in the AM parts of each track as these strict time constraint
            foreach (Track track in tracks)
            {
                var amBuilderResult = AMBuilder(talks);
                talks = amBuilderResult.remainingTalks;
                track.AMTalks = amBuilderResult.amTalks;
            }

            //fill out PM parts of the track afterwards
            foreach (Track track in tracks)
            {
                var pmBuilderResult = PMBuilder(talks);
                talks = pmBuilderResult.remainingTalks;
                track.PMTalks = pmBuilderResult.pmTalks;
            }

            return (tracks, talks);
        }

        public int getMinimumTime()
        {
            return AmHours + PmHoursMin;
        }

        //Private method to build the AM Parts of the tracks
        private (List<Talk> amTalks, List<Talk> remainingTalks) AMBuilder(List<Talk> talks)
        {
            
            TimeSpan currentDuration = TimeSpan.Zero;
            List<Talk> result = new List<Talk>();
            List<Talk> holdingList = new List<Talk>();
            
            //Loop to keep looking for talks which can still fit in the AM part
            //as long as their are still tracks available and the time requirement hasn't been met
            while (talks.Where(talk => (talk.Duration + currentDuration).TotalHours <= AmHours)
                .Select(talk => talk)
                .Any() 
                && TimeSpan.Compare(currentDuration, TimeSpan.FromHours(AmHours)) != 0)
            {
                //Query looking for the longest talk which can still fit
                var query = talks
                    .Where(talk => (talk.Duration + currentDuration).TotalHours <= AmHours)
                    .OrderByDescending(talk => talk.Duration)
                    .Select(talk => talk)
                    .First();
                
                
                talks.Remove(query);
                currentDuration += query.Duration;
                
                //Check to see if when adding this talk to the track, there are still talks available so we can meet our time requirement
                //If not, this talk, and all talks of the same duration are put in a holding list to be used for the other tracks
                
                //The check first checks whether the time constraint has been met
                //Then whether the remaining time if the selected talk is added is less than the max length of the remaining talks
                //Finally if their are any talks still available which could satisfy our time requirement
                if (currentDuration.TotalHours != AmHours 
                    && 
                    AmHours - currentDuration.TotalHours < talks.Max(t => t.Duration).TotalHours
                    &&
                    !talks
                    .Where(talk => (talk.Duration + currentDuration).TotalHours == AmHours)
                    .Select(talk => talk).Any())
                {
                    currentDuration -= query.Duration;
                    holdingList.Add(query);
                    holdingList.AddRange(talks.FindAll(talk => talk.Duration.Equals(query.Duration)));
                    talks.RemoveAll(talk => talk.Duration.Equals(query.Duration));
                }
                else
                {
                    result.Add(query); 
                }
                
                
                
            }
            List<Talk> remainingTalks = talks;
            remainingTalks.AddRange(holdingList);
            return (amTalks: result, remainingTalks: remainingTalks);
        }
        
        //Private method to build the PM Parts of the tracks
        private (List<Talk> pmTalks, List<Talk> remainingTalks) PMBuilder(List<Talk> talks)
        {
            TimeSpan pmDuration = TimeSpan.Zero;
            List<Talk> result = new List<Talk>();
            
            //Loop to keep looking for talks which fit in the time constraints while the max time isn't met
            while (TimeSpan.Compare(pmDuration, TimeSpan.FromHours(PmHoursMax)) < 0 
                   &&
                   talks.Where(talk => (talk.Duration + pmDuration).TotalHours <= PmHoursMax)
                       .Select(talk => talk)
                       .Any())
            {
                Talk query = talks
                    .Where(talk => (talk.Duration + pmDuration).TotalHours <= PmHoursMax)
                    .Select(talk => talk)
                    .First();

                pmDuration += query.Duration;
                result.Add(query);
                talks.Remove(query);
            }
            List<Talk> remainingTalks = talks;
            return (pmTalks: result, remainingTalks: remainingTalks);
        }
    }
}