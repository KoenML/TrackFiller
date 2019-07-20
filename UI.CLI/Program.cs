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
            int input = -1;
            ArrayList talks = new ArrayList();
            ArrayList tracks = new ArrayList();
            do
            {
                Console.Out.WriteLine("***********************************************");
                Console.Out.WriteLine("*          Welcome to  TrackBuilder           *");
                Console.Out.WriteLine("***********************************************");
                Console.Out.WriteLine("* Please Choose one of the following actions  *");
                Console.Out.WriteLine("***********************************************");
                Console.Out.WriteLine("* 1)\tLoad Talks (from file/manually)      *");
                Console.Out.WriteLine("* 2)\tBuild Tracks from loaded Talks       *");
                Console.Out.WriteLine("* 3)\tWrite Tracks to file                 *");
                Console.Out.WriteLine("* 0)\tExit (Losing all loaded Talks)       *");
                Console.Out.WriteLine("***********************************************");
                Console.Out.Write("Your input:\t");
                string inputString = Console.In.ReadLine();
                if (Int32.TryParse(inputString, out input))
                {
                    switch (input)
                    {
                        case 1: talks = LoadTalks();
                            break;
                        case 2: tracks = BuildTracks(talks);
                            break;
                        case 3: WriteTracks(tracks);
                            break;
                        default: Console.Out.WriteLine("Input not recognized, use one of the above menu options");
                            break;
                    }
                }
                else
                {
                    Console.Out.WriteLine("Input not recognized, strictly use the number in front of the menu option");
                }
                    
            } while (input != 0);

        }

        private static ArrayList LoadTalks()
        {
            int input = -1;
            ArrayList result = new ArrayList();
            ITalkReader talkReader;
            do
            {
                Console.Out.WriteLine("***********************************************");
                Console.Out.WriteLine("* Please Choose one of the following actions  *");
                Console.Out.WriteLine("***********************************************");
                Console.Out.WriteLine("* 1)\tLoad Talks from file (.txt)          *");
                Console.Out.WriteLine("* 2)\tLoad Talks manually                  *");
                Console.Out.WriteLine("* 0)\tSave and exit to main menu           *");
                Console.Out.WriteLine("*-1)\tExit without saving                  *");
                Console.Out.WriteLine("***********************************************");
                Console.Out.Write("Your input:\t");
                string inputString = Console.In.ReadLine();
                if (Int32.TryParse(inputString, out input))
                {
                    switch (input)
                    {
                        case 1: talkReader = new TxtTalkReader();
                            int count = result.Count;
                            result.AddRange(talkReader.readTalks());
                            Console.Out.WriteLine($"Added {result.Count - count} Talks");
                            break;
                        case 2: talkReader = new ManualTalkReader();
                            count = result.Count;
                            result.AddRange(talkReader.readTalks());
                            Console.Out.WriteLine($"Added {result.Count - count} Talks");
                            break;
                        case -1: return new ArrayList();
                        default: Console.Out.WriteLine("Input not recognized, use one of the above menu options");
                            break;

                    }
                }
                else
                {
                    Console.Out.WriteLine("Input not recognized, strictly use the number in front of the menu option");
 
                }
            } while (input != 0);
            
            return result;
        }

        private static ArrayList BuildTracks(ArrayList talks)
        {
            Console.Out.WriteLine("Gonna build those Tracks");
            return new ArrayList();
        }

        private static void WriteTracks(ArrayList tracks)
        {
            Console.Out.WriteLine("Gonna write those tracks");
        }

        /*
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
        */
    }
}