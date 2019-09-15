using System.Collections.Generic;
using PackageGraph.Library.Models;

namespace PackageGraph.Library.Interfaces
{
    public interface IInjector
    {
        IDictionary<string, Project> Inject(IDictionary<string, Project> nodes, List<string> projectsToAddContracts);
    }
}