using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using PackageGraph.Library.Models;

namespace PackageGraph.Library
{
    public class DirectoryScanner
    {
        private readonly ICommandLogger _logger;

        public DirectoryScanner(ICommandLogger logger)
        {
            _logger = logger;
        }

        public void ScanFolders(ProcessingOptions options)
        {
            if (!Directory.Exists(options.RootDiretory))
                return;

            var projectFiles = Directory.GetFiles(options.RootDiretory, "*.csproj",
                SearchOption.AllDirectories);

            foreach (var pf in projectFiles)
            {
                var name = Path.GetFileNameWithoutExtension(pf);

                if (options.ProjectsFilter != null)
                {
                    if (options.ProjectsFilter.MatchingType == MatchingType.Exclusion &&
                        options.ProjectsFilter.Names.Any(filtered => name.Contains(filtered)))
                        continue;
                    if (options.ProjectsFilter.MatchingType == MatchingType.Inclusion &&
                        options.ProjectsFilter.Names.Any(filtered => name.Contains(filtered)))
                    {
                        //TODO: something else
                    }
                }

                _logger.AddProject(name);

                var dependencies = GetPackages(Path.GetDirectoryName(pf));

                if (options.PackagesFilter != null)
                {
                    if (options.PackagesFilter.MatchingType == MatchingType.Exclusion)
                        dependencies = dependencies
                            .Where(d => !options.PackagesFilter.Names.Any(filtered => d.Contains(filtered))).ToList();

                    if (options.PackagesFilter.MatchingType == MatchingType.Inclusion)
                        dependencies = dependencies
                            .Where(d => options.PackagesFilter.Names.Any(filtered => d.Contains(filtered))).ToList();
                }

                foreach (var dependencyName in dependencies)
                {
                    _logger.AddProject(dependencyName);
                    _logger.AddDependency(name, dependencyName);
                }
            }
        }

        private static List<string> GetPackages(string path)
        {
            var packageFile = path + @"\packages.config";
            var result = new List<string>();
            if (!File.Exists(packageFile)) return result;

            foreach (var pr in XDocument.Load(packageFile).Descendants("package"))
                result.Add(pr.Attribute("id").Value);
            return result;
        }
    }
}