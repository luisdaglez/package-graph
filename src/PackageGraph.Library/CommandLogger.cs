using System.Collections.Generic;
using System.Linq;
using PackageGraph.Library.Interfaces;
using PackageGraph.Library.Models;

namespace PackageGraph.Library
{
    public class CommandLogger : ICommandLogger
    {
        private readonly List<CommandLog> _commands = new List<CommandLog>();

        public List<CommandLog> GetLogs()
        {
            return _commands;
        }

        public void AddProject(string name)
        {
            if (_commands.Any(c => c.Project == name &&
                                   c.CommandType == CommandType.AddProject))
                return;

            _commands.Add(new CommandLog
            {
                CommandType = CommandType.AddProject,
                Project = name
            });
        }

        public void AddDependency(string projectName, string dependencyName)
        {
            if (_commands.Any(c => c.Project == projectName &&
                                   c.Dependency == dependencyName &&
                                   c.CommandType == CommandType.AddDependency))
                return;

            _commands.Add(new CommandLog
            {
                CommandType = CommandType.AddDependency,
                Project = projectName,
                Dependency = dependencyName
            });
        }
    }
}