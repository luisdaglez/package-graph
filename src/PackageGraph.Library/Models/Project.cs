using System.Collections.Generic;

namespace PackageGraph.Library.Models
{
    public class Project
    {
        public Project()
        {
            Dependencies = new List<Project>();
            Dependents = new List<Project>();
        }

        public string Name { get; set; }
        public int DepthLevel { get; set; }
        public bool Orphaned => Dependencies.Count == 0 && Dependents.Count == 0;
        public List<Project> Dependencies { get; set; }
        public List<Project> Dependents { get; set; }
    }
}