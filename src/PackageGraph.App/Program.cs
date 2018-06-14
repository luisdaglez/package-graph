using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PackageGraph.Library;
using PackageGraph.Library.Models;

namespace PackageGraph.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var processingOptions = new ProcessingOptions
            {
                RootDiretory = @"C:\source",
                ProjectsFilter = new NameMatchingFilter
                {
                    Names = new List<string> {"Test"},
                    MatchingType = MatchingType.Exclusion
                },
                PackagesFilter = new NameMatchingFilter
                {
                    Names = new List<string>
                    {
                        "Microsoft",
                        "System",
                        "log4net",
                        "bootstrap",
                        "MSBuild",
                        "Newtonsoft.Json",
                        "Unity",
                        "Slowcheetah",
                        "Modernizr",
                        "jQuery",
                        "knockout",
                        "NServiceBus",
                        "Nancy",
                        "OctoPack",
                        "TinyIoC",
                        "CommonServiceLocator",
                        "AntiXSS",
                    },
                    MatchingType = MatchingType.Exclusion
                }
            };

            var displayOptions = new DgmlOptions
            {
                CellWidth = 380,
                CellHeight = 60,
                ShowOrphans = true,
                OutputPath = Path.Combine(Directory.GetCurrentDirectory() + @"\..\..\..\Graph.dgml"),
            };

            ICommandLogger logger = new CommandLogger();
            var scanner = new DirectoryScanner(logger);
            scanner.ScanFolders(processingOptions);

            IGraphBuilder builder = new GraphBuilder();
            builder.BuildGraph(logger);
            var nodes = builder.GetNodesSorted(GraphSorting.ClusterToRoots);

            Console.WriteLine("total: " + nodes.Count);
            Console.WriteLine("orphans: " + nodes.Count(p => p.Orphaned));
            Console.WriteLine("ymax: " + nodes.Where(p => !p.Orphaned).GroupBy(l => l.DepthLevel).Max(g => g.Count()));
            Console.WriteLine("xmax: " + nodes.Max(x => x.DepthLevel));

            var dgmlGenerator = new DgmlGenerator();
            dgmlGenerator.GenerateDgml(nodes, displayOptions);

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}
