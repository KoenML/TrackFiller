using System;
using System.Collections;
using BL;
using Domain;

namespace TrackFiller
{
    class Program
    {
        static void Main(string[] args)
        {
            ITalkReader talkReader = new TxtTalkReader();
            ArrayList talks = talkReader.readTalks(@"C:\Users\koenm\RiderProjects\TrackFiller\input.txt");
            
            TrackBuilder builder = new TrackBuilder(3, 4, 5);
            var builderResult = builder.BuildTrack(talks);
            Track firstTrack = builderResult.track;
            ArrayList remainingTalks = builderResult.remainingTalks;
            Console.Out.WriteLine("AM TALKS:");
            foreach (Talk amTalk in firstTrack.AMTalks)
            {
                Console.Out.WriteLine($"talk: {amTalk.Title}\t duration: {amTalk.Duration.Hours}h{amTalk.Duration.Minutes}m");
            }
            Console.Out.WriteLine("PM TALKS:");

            foreach (Talk amTalk in firstTrack.PMTalks)
            {
                Console.Out.WriteLine($"talk: {amTalk.Title}\t duration: {amTalk.Duration.Hours}h{amTalk.Duration.Minutes}m");
            }
            Console.Out.WriteLine("NON INCLUDED TALKS:");
            foreach (Talk amTalk in remainingTalks)
            {
                Console.Out.WriteLine($"talk: {amTalk.Title}\t duration: {amTalk.Duration.Hours}h{amTalk.Duration.Minutes}m");
            }
            
        }
    }
}