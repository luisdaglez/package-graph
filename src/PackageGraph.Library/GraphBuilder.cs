using System.Collections.Generic;
using System.Linq;
using PackageGraph.Library.Models;

namespace PackageGraph.Library
{
    public class GraphBuilder : IGraphBuilder
    {
        private readonly IDictionary<string, Project> _projects = new Dictionary<string, Project>();

        public void BuildGraph(ICommandLogger logger)
        {
            var projects = logger
                .GetLogs()
                .Where(l => l.CommandType == CommandType.AddProject)
                .OrderBy(l => l.Project);
            foreach (var item in projects)
                AddProject(item.Project);

            var dependencies = logger
                .GetLogs()
                .Where(l => l.CommandType == CommandType.AddDependency)
                .OrderBy(l => l.Project);
            foreach (var item in dependencies)
                AddDependency(item.Project, item.Dependency);
        }

        public List<Project> GetNodesSorted(GraphSorting sorting)
        {
            var nodes = _projects.Values.Where(n => !n.Orphaned).ToList();

            IncreaseDepth(nodes, 0, sorting);

            return _projects.Values.ToList();
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

        private void AddProject(string name)
        {
            if (_projects.ContainsKey(name))
                return;

            _projects.Add(name, new Project
            {
                Name = name
            });
        }

        private Project GetProject(string name)
        {
            return _projects.ContainsKey(name) ? _projects[name] : null;
        }

        private void AddDependency(string projectName, string dependencyName)
        {
            var project = GetProject(projectName);
            var dependency = GetProject(dependencyName);

            project.Dependencies.Add(dependency);
            dependency.Dependents.Add(project);
        }
    }
}