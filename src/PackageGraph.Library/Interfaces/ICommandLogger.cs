using System.Collections.Generic;
using PackageGraph.Library.Models;

namespace PackageGraph.Library.Interfaces
{
    public interface ICommandLogger
    {
        void AddProject(string projectName);
        void AddDependency(string projectName, string dependencyName);
        List<CommandLog> GetLogs();
    }
}