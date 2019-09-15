using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PackageGraph.Library;
using PackageGraph.Library.Interfaces;
using PackageGraph.Library.Models;

namespace PackageGraph.App
{
    internal class Program
    {
        private static readonly List<string> ExcludedItems = new List<string>
        {
            "System.*", "Microsoft.*", "*Tests"
        };

        private static readonly List<string> ProjectsToAddContractLayer = new List<string>
        {
        };

        private static void Main(string[] args)
        {
            var config = new AppConfiguration
            {
                RootDirectoryToScan = @"C:\source",
                CellWidth = 400,
                CellHeight = 50,
                ShowOrphans = true,
                OutputPath = Path.Combine(Directory.GetCurrentDirectory() + @"\..\..\..\Graph.dgml"),
                GraphSorting = GraphSorting.ClusterToRoots,
                ExcludedItems = ExcludedItems
            };

            ICommandLogger logger = new CommandLogger();
            IDependencyExtractor extractor = new DependencyExtractor();
            var scanner = new DirectoryScanner(logger, config, extractor);
            scanner.ScanFolders();
            var logs = logger.GetLogs();


            IGraphBuilder builder = new GraphBuilder(config);
            var connectedNodes = builder.BuildGraph(logs);

            IInjector injector = new Injector();
            connectedNodes = injector.Inject(connectedNodes, ProjectsToAddContractLayer);

            IGraphSorter sorter = new GraphSorter(config);
            var nodes = sorter.GetSortedNodes(connectedNodes);

            Console.WriteLine("total: " + nodes.Count);
            Console.WriteLine("orphans: " + nodes.Count(p => p.Orphaned));
            Console.WriteLine("ymax: " + nodes.Where(p => !p.Orphaned).GroupBy(l => l.DepthLevel).Max(g => g.Count()));
            Console.WriteLine("xmax: " + nodes.Max(x => x.DepthLevel));

            var dgmlGenerator = new DgmlGenerator();
            dgmlGenerator.GenerateDgml(nodes, config);

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}