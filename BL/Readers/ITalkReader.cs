using System.Collections.Generic;
using Domain;

namespace BL
{
    public interface ITalkReader
    {
        List<Talk> readTalks();
    }
}