using System.Collections.Generic;

namespace PackageGraph.Library.Models
{
    public class NameMatchingFilter
    {
        public MatchingType MatchingType { get; set; }
        public List<string> Names { get; set; }
    }
}