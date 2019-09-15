using System.Collections.Generic;
using PackageGraph.Library.Models;

namespace PackageGraph.Library.Interfaces
{
    public interface IGraphBuilder
    {
        IDictionary<string, Project> BuildGraph(List<CommandLog> logs);
    }
}