using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using PackageGraph.Library.Interfaces;

namespace PackageGraph.Library
{
    public class DependencyExtractor : IDependencyExtractor
    {
        public List<string> GetPackages(string projectPath)
        {
            var result = new List<string>();

            //traditional packages
            var packageFile = Path.GetDirectoryName(projectPath) + @"\packages.config";
            if (File.Exists(packageFile))
                foreach (var pr in XDocument.Load(packageFile).Descendants("package"))
                    result.Add(pr.Attribute("id").Value);

            //PackageReference in csproj
            foreach (var pr in XDocument.Load(projectPath).Descendants("PackageReference"))
                result.Add(pr.Attribute("Include").Value);

            return result;
        }
    }
}