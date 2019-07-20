using System;
using System.Collections;
using Domain;

namespace BL
{
    public class ManualTalkReader : ITalkReader
    {
        public ArrayList readTalks()
        {
            ArrayList talks = new ArrayList();
            int input = -1;
            do
            {
                Console.Out.WriteLine("***********************************************");
                Console.Out.WriteLine("* Please Choose one of the following actions  *");
                Console.Out.WriteLine("***********************************************");
                Console.Out.WriteLine("* 1)\tEnter Talk                           *");
                Console.Out.WriteLine("* 0)\tSave and exit to previous menu       *");
                Console.Out.WriteLine("*-1)\tExit to previous menu                *");
                Console.Out.WriteLine("***********************************************");
                Console.Out.Write("Your input:\t");
                string inputString = Console.In.ReadLine();
                if (Int32.TryParse(inputString, out input))
                {
                    switch (input)
                    {
                        case 1: talks.Add(readTalk());
                            break;
                        case -1: return new ArrayList();
                    }
                }
                
            } while (input != 0);

            return talks;
        }

        private Talk readTalk()
        {
            string correct = "n";
            string title;
            int duration;
            do
            {
                Console.Out.WriteLine("Enter talk title");
                title = Console.In.ReadLine();
                Console.Out.WriteLine("Is this title correct");
                Console.Out.WriteLine(title);
                Console.Out.WriteLine("(y/n)");
                correct = Console.In.ReadLine();
            } while (!correct.Equals("y"));
            correct = "n";
            do
            {
                Console.Out.WriteLine("Enter talk duration in minutes");
                string durationString = Console.In.ReadLine();
                if (Int32.TryParse(durationString, out duration) && duration > 0)
                {
                    Console.Out.WriteLine("Is this duration correct");
                    Console.Out.WriteLine(duration);
                    Console.Out.WriteLine("(y/n)");
                    correct = Console.In.ReadLine();
                }
                else
                {
                    Console.Out.WriteLine("Could not parse duration, only use positive numbers");
                }
                
            } while (!correct.Equals("y"));
            
            return new Talk(title, TimeSpan.FromMinutes(duration));
        }
    }
}