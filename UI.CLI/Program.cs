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
            foreach (Talk talk in talks)
            {
                Console.WriteLine($"talk:{talk.Title} duration:{talk.Duration.Hours} h {talk.Duration.Minutes} min");
            }
        }
    }
}