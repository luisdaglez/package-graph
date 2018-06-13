using System.Collections.Generic;
using PackageGraph.Library.Models;

namespace PackageGraph.Library
{
    public interface ICommandLogger
    {
        void AddProject(string name);
        void AddDependency(string projectName, string dependencyName);
        List<CommandLog> GetLogs();
    }
}