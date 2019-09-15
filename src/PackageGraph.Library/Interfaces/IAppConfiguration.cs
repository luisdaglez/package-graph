using System.Collections.Generic;
using PackageGraph.Library.Models;

namespace PackageGraph.Library.Interfaces
{
    public interface IAppConfiguration
    {
        string RootDirectoryToScan { get; }
        int CellWidth { get; }
        int CellHeight { get; }
        bool ShowOrphans { get; }
        string OutputPath { get; }
        GraphSorting GraphSorting { get; }
        List<string> ExcludedItems { get; }
    }
}