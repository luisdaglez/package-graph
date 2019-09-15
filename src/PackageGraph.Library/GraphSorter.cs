using System.Collections.Generic;
using System.Linq;
using PackageGraph.Library.Interfaces;
using PackageGraph.Library.Models;

namespace PackageGraph.Library
{
    public class GraphSorter : IGraphSorter
    {
        private readonly IAppConfiguration _config;

        public GraphSorter(IAppConfiguration config)
        {
            _config = config;
        }
        public List<Project> GetSortedNodes(IDictionary<string, Project> nodes)
        {
            var nodeList = nodes.Values.Where(n => !n.Orphaned).ToList();

            IncreaseDepth(nodeList, 0, _config.GraphSorting);

            return nodes.Values.ToList();
        }

        private void IncreaseDepth(List<Project> nodes, int currentLevel, GraphSorting sorting)
        {
            foreach (var node in nodes)
                if (node.DepthLevel <= currentLevel)
                {
                    node.DepthLevel = currentLevel + 1;
                    IncreaseDepth(
                        sorting == GraphSorting.ClusterToLeaves
                            ? node.Dependents
                            : node.Dependencies,
                        currentLevel + 1,
                        sorting);
                }
        }
    }
}