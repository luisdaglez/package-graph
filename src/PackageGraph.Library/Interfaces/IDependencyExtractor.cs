using System.Collections.Generic;

namespace PackageGraph.Library.Interfaces
{
    public interface IDependencyExtractor
    {
        List<string> GetPackages(string projectPath);
    }
}