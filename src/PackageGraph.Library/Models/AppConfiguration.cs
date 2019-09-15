using System.Collections.Generic;
using PackageGraph.Library.Interfaces;

namespace PackageGraph.Library.Models
{
    public class AppConfiguration : IAppConfiguration
    {
        public string RootDirectoryToScan { get; set; }
        public int CellWidth { get; set; }
        public int CellHeight { get; set; }
        public bool ShowOrphans { get; set; }
        public string OutputPath { get; set; }
        public GraphSorting GraphSorting { get; set; }
        public List<string> ExcludedItems { get; set;  }
    }
}