using System;
using System.Collections.Generic;
using System.Linq;
using BL;
using BL.Builders;
using BL.Exceptions;
using BL.Writers;
using Domain;

namespace TrackFiller
{
    class Program
    {
        static void Main(string[] args)
        {
            int input = 0;
            List<Talk> talks = new List<Talk>();
            List<Track> tracks = new List<Track>();
            
            
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

        private static List<Talk> LoadTalks()
        {
            int input = 0;
            List<Talk> result = new List<Talk>();
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
                    Console.Out.WriteLine("Input not recognized, use one of the above menu options");
 
                }
            } while (input != -1);
            
            return result;
        }

        private static (List<Track> tracks, List<Talk> remainingTalks) BuildTracks(List<Talk> talks)
        {
            int input = 0;
            List<Track> result = new List<Track>();
            ITrackBuilder trackBuilder = new SimulTrackBuilder(3, 3, 4);
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
                            trackBuilder = TrackBuilderSettings();
                            break;
                        case 2:
                            if (talks.Count == 0)
                            {
                                Console.Out.WriteLine("There are no talks available to build new track");
                                break;
                            }
                            Console.Out.WriteLine("How many Tracks should be build?");
                            inputString = Console.In.ReadLine();
                            if (Int32.TryParse(inputString, out input) && input > 0)
                            {
                                try
                                {
                                    var builderResult = trackBuilder.BuildTracks(talks, input);
                                    result = builderResult.tracks;
                                    talks = builderResult.remainingTalks;
                                    if (talks.Count != 0)
                                    {
                                        Console.Out.WriteLine($"There are still {talks.Count} talks left not included in any tracks");
                                        Console.Out.WriteLine($"You can add {trackBuilder.getMinimumTime() * 60 - talks.Sum(t => t.Duration.TotalMinutes)} minutes worth of talks to create another track");
                                        Console.In.Read();
                                    }
                                }
                                catch (NotEnoughTalksException e)
                                {
                                    Console.WriteLine($"Not enough talks to fill requested number of tracks. {e.MinutesShort} minutes short");
                                    Console.In.ReadLine();
                                }
                            }
                            else
                            {
                                Console.Out.WriteLine("Input not recognized, use only positive integers");
                            }
                            break;
                        default: Console.Out.WriteLine("Input not recognized, use one of the above menu options");
                            break;
                    }
                }
                else
                {
                    Console.Out.WriteLine("Input not recognized, use one of the above menu options");
                }
            } while (input != -1);
            
            return (tracks: result, remainingTalks: talks);
        }

        private static void WriteTracks(List<Track> tracks)
        {
            ITrackWriter trackWriter = new TxtTrackWriter();
            trackWriter.writeTracks(tracks);
        }
        
        private static ITrackBuilder TrackBuilderSettings()
        {
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
                            return new SimulTrackBuilder(amHours, pmMin, pmMax);
        }
    }
}