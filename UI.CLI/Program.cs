using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using BL;
using BL.Writers;
using Domain;

namespace TrackFiller
{
    class Program
    {
        static void Main(string[] args)
        {
            int input = 0;
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
                Console.Out.WriteLine("*-1)\tExit (Losing all loaded Talks)       *");
                Console.Out.WriteLine("***********************************************");
                Console.Out.Write("Your input:\t");
                string inputString = Console.In.ReadLine();
                if (Int32.TryParse(inputString, out input))
                {
                    switch (input)
                    {
                        case 1: talks = LoadTalks();
                            break;
                        case 2:
                            var buildResult = BuildTracks(talks);
                            tracks = buildResult.tracks;
                            talks = buildResult.remainingTalks;
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
                    
            } while (input != -1);

        }

        private static ArrayList LoadTalks()
        {
            int input = 0;
            ArrayList result = new ArrayList();
            ITalkReader talkReader;
            do
            {
                Console.Out.WriteLine("***********************************************");
                Console.Out.WriteLine("* Please Choose one of the following actions  *");
                Console.Out.WriteLine("***********************************************");
                Console.Out.WriteLine("* 1)\tLoad Talks from file (.txt)          *");
                Console.Out.WriteLine("* 2)\tLoad Talks manually                  *");
                Console.Out.WriteLine("*-1)\tSave and exit to main menu           *");
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
                            Console.Out.WriteLine($"Loaded {result.Count - count} Talks");
                            break;
                        case 2: talkReader = new ManualTalkReader();
                            count = result.Count;
                            result.AddRange(talkReader.readTalks());
                            Console.Out.WriteLine($"Loaded {result.Count - count} Talks");
                            break;
                        default: Console.Out.WriteLine("Input not recognized, use one of the above menu options");
                            break;

                    }
                }
                else
                {
                    Console.Out.WriteLine("Input not recognized, strictly use the number in front of the menu option");
 
                }
            } while (input != -1);
            
            return result;
        }

        private static (ArrayList tracks, ArrayList remainingTalks) BuildTracks(ArrayList talks)
        {
            int input = 0;
            ArrayList result = new ArrayList();
            TrackBuilder trackBuilder = new TrackBuilder(3, 3, 4);
            do
            {
                Console.Out.WriteLine("***********************************************");
                Console.Out.WriteLine("* Please Choose one of the following actions  *");
                Console.Out.WriteLine("***********************************************");
                Console.Out.WriteLine("* 1)\tAdjust Default Track Setting         *");
                Console.Out.WriteLine("* 2)\tBuild Tracks                         *");
                Console.Out.WriteLine("*-1)\tSave and exit to main menu           *");
                Console.Out.WriteLine("***********************************************");
                Console.Out.Write("Your input:\t");
                string inputString = Console.In.ReadLine();
                if (Int32.TryParse(inputString, out input))
                {
                    switch (input)
                    {
                        case 1: 
                            Console.Out.WriteLine("How many hours are in the AM session: ");
                            string settingsString = Console.In.ReadLine();
                            if (!Int32.TryParse(settingsString, out int amHours) && amHours != 0)
                            {
                                do
                                {
                                    Console.Out.WriteLine("Invalid input (cannot be 0)");
                                    Console.Out.WriteLine("How many hours are in the AM session: ");
                                    settingsString = Console.In.ReadLine();
                                } while (!Int32.TryParse(settingsString, out amHours) && amHours != 0);
                            }
                            Console.Out.WriteLine("How many hours are minimum in the PM session: ");
                            settingsString = Console.In.ReadLine();
                            if (!Int32.TryParse(settingsString, out int pmMin) && pmMin != 0)
                            {
                                do
                                {
                                    Console.Out.WriteLine("Invalid input (cannot be 0)");
                                    Console.Out.WriteLine("How many hours are minimum in the PM session: ");
                                    settingsString = Console.In.ReadLine();
                                } while (!Int32.TryParse(settingsString, out pmMin) && pmMin != 0);
                            }
                            Console.Out.WriteLine("How many hours are maximum in the PM session: ");
                            settingsString = Console.In.ReadLine();
                            if (!Int32.TryParse(settingsString, out int pmMax) && pmMax != 0)
                            {
                                do
                                {
                                    Console.Out.WriteLine("Invalid input (cannot be 0)");
                                    Console.Out.WriteLine("How many hours are maximum in the PM session: ");
                                    settingsString = Console.In.ReadLine();
                                } while (!Int32.TryParse(settingsString, out pmMax) && pmMax != 0);
                            }
                            trackBuilder = new TrackBuilder(amHours, pmMin, pmMax);
                            break;
                        case 2:
                            if (talks.Count == 0)
                            {
                                Console.Out.WriteLine("There are no more talks available to build new track");
                                break;
                            }
                            Console.Out.WriteLine($"There are {talks.Count} available talks to build a new Track");
                            Console.Out.WriteLine("Do you want to build a new Track? y/n (no exits this menu)");
                            string newTrackAnswer = Console.In.ReadLine();
                            switch (newTrackAnswer)
                            {
                                case "y":
                                    var trackAndRemaining = trackBuilder.BuildTrack(talks);
                                    result.Add(trackAndRemaining.track);
                                    talks = trackAndRemaining.remainingTalks;
                                    Console.Out.WriteLine($"Track built, {trackAndRemaining.remainingTalks.Count} remaining talks");
                                    break;
                                case "n":
                                    input = -1;
                                    break;
                                default:
                                    Console.Out.WriteLine("Input not recognized");
                                    break;
                            }
                            break;
                        default: Console.Out.WriteLine("Input not recognized, use one of the above menu options");
                            break;
                    }
                }
                else
                {
                    Console.Out.WriteLine("Input not recognized, strictly use the number in front of the menu option");
                }
            } while (input != -1);
            
            return (tracks: result, remainingTalks: talks);
        }

        private static void WriteTracks(ArrayList tracks)
        {
            ITrackWriter trackWriter = new TxtTrackWriter();
            trackWriter.writeTracks(tracks);
        }
    }
}