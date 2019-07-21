using System.Collections;
using System.Collections.Generic;
using Domain;

namespace BL.Builders
{
    public interface ITrackBuilder
    {
        (List<Track> tracks, List<Talk> remainingTalks) BuildTracks(List<Talk> talks, int numberOfTracks);
        int getMinimumTime();
    }
}