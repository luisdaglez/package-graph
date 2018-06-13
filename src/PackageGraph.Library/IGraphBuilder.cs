using System.Collections.Generic;
using PackageGraph.Library.Models;

namespace PackageGraph.Library
{
    public interface IGraphBuilder
    {
        void BuildGraph(ICommandLogger logger);
        List<Project> GetNodesSorted(GraphSorting sorting);
    }
}