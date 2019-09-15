using System.Collections.Generic;
using PackageGraph.Library.Interfaces;
using PackageGraph.Library.Models;

namespace PackageGraph.Library
{
    public class Injector : IInjector
    {
        public IDictionary<string, Project> Inject(IDictionary<string, Project> nodes,
            List<string> projectsToAddContracts)
        {
            var projects = nodes;
            foreach (var mainProjectName in projectsToAddContracts) projects = Inject(projects, mainProjectName);

            return projects;
        }

        private IDictionary<string, Project> Inject(IDictionary<string, Project> nodes, string mainProjectName)
        {
            IDictionary<string, Project> _projects = new Dictionary<string, Project>();

            foreach (var mainProject in nodes)
            {
                var contractProjectName = mainProjectName + ".Contracts";
                if (mainProject.Key == mainProjectName)
                {
                    var contract = new KeyValuePair<string, Project>(contractProjectName,
                        new Project
                        {
                            Name = contractProjectName,
                            Dependencies = new List<Project>(),
                            Dependents = mainProject.Value.Dependents
                        });
                    _projects.Add(contract);

                    foreach (var dependent in mainProject.Value.Dependents)
                        dependent.Dependencies.Add(contract.Value);

                    // now lets remove all the main project's dependents that are not top level
                    foreach (var dependent in mainProject.Value.Dependents.ToArray())
                        if (dependent.Dependents.Count > 0)
                        {
                            mainProject.Value.Dependents.Remove(dependent);
                            dependent.Dependencies.Remove(mainProject.Value);
                        }

                    //add inter-dependencies between main and contract
                    mainProject.Value.Dependencies.Add(contract.Value);
                    contract.Value.Dependents.Add(mainProject.Value);
                }

                _projects.Add(mainProject);
            }

            return _projects;
        }
    }
}