using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Domain;

namespace BL.Writers
{
    public class TxtTrackWriter : ITrackWriter
    {
        public void writeTracks(List<Track> tracks)
        {
            int startingHourAm;
            int startingHourPm;
            
            
            Console.Out.WriteLine("What hour does the AM session start: ");
            string settingsString = Console.In.ReadLine();
            if (!Int32.TryParse(settingsString, out startingHourAm) && startingHourAm != 0)
            {
                do
                {
                    Console.Out.WriteLine("Invalid input (cannot be 0)");
                    Console.Out.WriteLine("What hour does the AM session start: ");
                    settingsString = Console.In.ReadLine();
                } while (!Int32.TryParse(settingsString, out startingHourAm) && startingHourAm != 0);
            }
            Console.Out.WriteLine("What hour does the PM session start: ");
            settingsString = Console.In.ReadLine();
            if (!Int32.TryParse(settingsString, out startingHourPm) && startingHourPm != 0)
            {
                do
                {
                    Console.Out.WriteLine("Invalid input (cannot be 0)");
                    Console.Out.WriteLine("What hour does the PM session start: ");
                    settingsString = Console.In.ReadLine();
                } while (!Int32.TryParse(settingsString, out startingHourPm) && startingHourPm != 0);
            }
            if (startingHourPm < 12)
            {
                startingHourPm += 12;
            }
            bool exists = false;
            string directoryPath;
            do
            {
                Console.Out.Write("Directory to write file in:\t");
                directoryPath = Console.In.ReadLine();
                if (System.IO.Directory.Exists(directoryPath))
                {
                    exists = true;
                    
                }
                else
                {
                    Console.WriteLine("Directory does not exist");
                }
            } while (!exists);
            string filepath = directoryPath + $"\\ConferenceTracks{DateTime.Now.Ticks}.txt";
            using (StreamWriter sw = File.CreateText(filepath))
            {
                sw.WriteLine("Conference Planning");
            }

            using (StreamWriter sw = File.AppendText(filepath))
            {
                sw.WriteLine("\n");
                sw.WriteLine("\n");
                int trackCounter = 1;
                foreach (Track track in tracks)
                {
                    sw.WriteLine($"Track {trackCounter}:");
                    TimeSpan amCounter = TimeSpan.FromHours(startingHourAm);
                    foreach (Talk talk in track.AMTalks)
                    {
                        sw.WriteLine($"{amCounter.Hours.ToString().PadLeft(2,'0')}:{amCounter.Minutes.ToString().PadLeft(2,'0')}AM : {talk.Title} {talk.Duration.Hours*60 + talk.Duration.Minutes} min");
                        amCounter += talk.Duration;
                    }
                    TimeSpan pmCounter = TimeSpan.FromHours(startingHourPm);
                    foreach (Talk talk in track.PMTalks)
                    {
                        sw.WriteLine($"{pmCounter.Hours.ToString().PadLeft(2,'0')}:{pmCounter.Minutes.ToString().PadLeft(2,'0')}PM : {talk.Title} {talk.Duration.Hours*60 + talk.Duration.Minutes} min");
                        pmCounter += talk.Duration;
                    }

                    trackCounter++;
                }
            }
            
            Console.Out.WriteLine("File Written, Press any key to return to main menu");
            Console.In.ReadLine();

        }
        
        
    }
}