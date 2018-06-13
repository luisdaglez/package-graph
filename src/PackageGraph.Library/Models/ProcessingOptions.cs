namespace PackageGraph.Library.Models
{
    public class ProcessingOptions
    {
        public string RootDiretory { get; set; }
        public NameMatchingFilter PackagesFilter { get; set; }
        public NameMatchingFilter ProjectsFilter { get; set; }
    }
}