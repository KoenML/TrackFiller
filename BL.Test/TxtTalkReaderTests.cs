using System;
using System.Collections;
using BL;
using Domain;
using NUnit.Framework;

namespace Tests
{
    public class TxtTalkReaderTests
    {
        private ITalkReader talkReader;
        private string filePath;
        
        [SetUp]
        public void Setup()
        {
            talkReader = new TxtTalkReader();
            filePath = @"C:\Users\koenm\RiderProjects\TrackFiller\BL.Test\testLines.txt";
            string[] testLines = new[]
            {
                "Writing Fast Tests Against Enterprise Rails 60min", 
                "Clojure Ate Scala (on my project) 45min",
                "Rails for Python Developers lightning"
            };
            System.IO.File.WriteAllLines(filePath, testLines);
        }

        [Test]
        public void ParseTest()
        {
            ArrayList talks = talkReader.readTalks(filePath);
            foreach (Talk talk in talks)
            {
                Assert.AreEqual(1, TimeSpan.Compare(talk.Duration, TimeSpan.Zero));
                Assert.IsNotEmpty(talk.Title);
            }
        }
    }
}