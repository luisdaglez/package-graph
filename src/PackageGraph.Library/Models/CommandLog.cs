namespace PackageGraph.Library.Models
{
    public class CommandLog
    {
        public CommandType CommandType { get; set; }
        public string Project { get; set; }
        public string Dependency { get; set; }
    }
}