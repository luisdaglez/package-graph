using System.Collections.Generic;
using PackageGraph.Library.Models;

namespace PackageGraph.Library.Interfaces
{
    public interface IGraphSorter
    {
        List<Project> GetSortedNodes(IDictionary<string, Project> nodes);
    }
}
