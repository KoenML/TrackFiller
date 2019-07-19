using System;
using System.Collections;
using System.Text.RegularExpressions;
using Domain;

namespace BL
{
    public class TxtTalkReader : ITalkReader
    {
        
        public ArrayList readTalks(string path)
        {
            //Read file as array of lines
            string[] lines = System.IO.File.ReadAllLines(path);
            
            //Initialize return value and pattern to match against for parsing lines
            ArrayList talks = new ArrayList();
            string pattern = @"(?'Title'[^0-9\n]+)(?'Duration'lightning|\d+)(min)?";
            
            //Iterate over lines, parse each line into title and duration
            foreach (string line in lines)
            {
                
                //Regex match against earlier defined pattern
                Match match = Regex.Match(line, pattern);
                
                //Use capturing groups to split match into title value and duration value
                string title = match.Groups["Title"].Value;
                string durationString = match.Groups["Duration"].Value;
                
                //Initialize Timespan to be used in Talk constructor
                TimeSpan duration;
                
                //Parse duration based on whether talk is lightning talk or not
                if (durationString == "lightning")
                {
                    duration = TimeSpan.FromMinutes(5);
                }
                else
                {
                    int minutes = Int32.Parse(durationString);
                    duration = TimeSpan.FromMinutes(minutes);
                }
                
                //Instantiate new talk using title and duration found on line and add to collection of talks
                Talk newTalk = new Talk(title,duration);
                talks.Add(newTalk);
            }
            
            //return array containing all talk objects as parsed from txt file
            return talks;
        }
    }
}