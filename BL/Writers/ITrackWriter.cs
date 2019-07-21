using System.Collections;
using System.Collections.Generic;
using Domain;

namespace BL.Writers
{
    public interface ITrackWriter
    {
        void writeTracks(List<Track> tracks);
    }
}