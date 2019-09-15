using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PackageGraph.Library.Interfaces;
using PackageGraph.Library.Models;

namespace PackageGraph.Library
{
    public class GraphBuilder : IGraphBuilder
    {
        private readonly IAppConfiguration _config;
        private readonly HashSet<string> _excludedProjects = new HashSet<string>();
        private readonly IDictionary<string, Project> _projects = new Dictionary<string, Project>();

        public GraphBuilder(IAppConfiguration config)
        {
            _config = config;
        }

        public IDictionary<string, Project> BuildGraph(List<CommandLog> logs)
        {
            var projects = logs.Where(l => l.CommandType == CommandType.AddProject);
            foreach (var item in projects)
                if (IsIncluded(item.Project))
                    AddProject(item.Project);

            var dependencies = logs.Where(l => l.CommandType == CommandType.AddDependency);
            foreach (var item in dependencies)
                if (IsIncluded(item.Project) && IsIncluded(item.Dependency))
                    AddDependency(item.Project, item.Dependency);

            Console.WriteLine(" -- Excluded projects --");
            foreach (var project in _excludedProjects.ToList().OrderBy(c => c)) Console.WriteLine(project);
            Console.WriteLine();

            return _projects;
        }

        private bool IsIncluded(string name)
        {
            if (_config.ExcludedItems.Contains(name))
            {
                _excludedProjects.Add(name);
                return false;
            }

            foreach (var excluded in _config.ExcludedItems)
                if (excluded.Contains("*"))
                {
                    var r = new Regex("^" + excluded.Replace(".", @"\.*").Replace("*", @".*"));
                    if (r.Match(name).Success)
                    {
                        _excludedProjects.Add(name);
                        return false;
                    }
                }

            return true;
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