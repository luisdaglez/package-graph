using System;
using System.IO;
using PackageGraph.Library.Interfaces;

namespace PackageGraph.Library
{
    public class DirectoryScanner
    {
        private readonly IAppConfiguration _config;
        private readonly IDependencyExtractor _extractor;
        private readonly ICommandLogger _logger;

        public DirectoryScanner(ICommandLogger logger, IAppConfiguration config, IDependencyExtractor extractor)
        {
            _logger = logger;
            _config = config;
            _extractor = extractor;
        }

        public void ScanFolders(bool isVerbose = false)
        {
            if (!Directory.Exists(_config.RootDirectoryToScan))
                return;

            var projectFiles = Directory.GetFiles(_config.RootDirectoryToScan, "*.csproj",
                SearchOption.AllDirectories);

            foreach (var pf in projectFiles)
            {
                if (isVerbose)
                    Console.WriteLine(pf);

                var name = Path.GetFileNameWithoutExtension(pf);

                _logger.AddProject(name);

                var dependencies = _extractor.GetPackages(pf);

                foreach (var dependencyName in dependencies)
                {
                    _logger.AddProject(dependencyName);
                    _logger.AddDependency(name, dependencyName);
                }
            }
        }
    }
}