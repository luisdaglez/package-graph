using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using PackageGraph.Library.Models;

namespace PackageGraph.Library
{
    public class DgmlGenerator
    {
        private readonly XNamespace _dgmlns = "http://schemas.microsoft.com/vs/2009/dgml";
        private DgmlOptions _options;

        public void GenerateDgml(IEnumerable<Project> projects, DgmlOptions options)
        {
            _options = options;
            var nodes = new List<XElement>();

            var linked = projects.Where(p => !p.Orphaned).ToList();
            var ymax = linked.GroupBy(l => l.DepthLevel).Max(g => g.Count());
            AddLinkedProjects(linked, nodes, ymax);

            if (options.ShowOrphans)
            {
                var orphans = projects.Where(p => p.Orphaned);
                AddOrphans(orphans, nodes, ymax);
            }

            var links = new List<XElement>();
            foreach (var project in projects)
            foreach (var dep in project.Dependencies)
                links.Add(GetLink(project.Name, dep.Name, "Project Reference"));

            var graph = new XElement(_dgmlns + "DirectedGraph",
                new XElement(_dgmlns + "Nodes", nodes),
                new XElement(_dgmlns + "Links", links)
            );
            var doc = new XDocument(graph);
            doc.Save(options.OutputPath);
        }

        private void AddLinkedProjects(IEnumerable<Project> linked, ICollection<XElement> nodes, int ymax)
        {
            foreach (var groups in linked.GroupBy(l => l.DepthLevel))
            {
                double y = 0;
                var itemsInLevel = groups.ToList();
                var level = groups.Key;
                foreach (var project in itemsInLevel)
                {
                    var xpos = level;
                    var ypos = itemsInLevel.Count == 1
                        ? (double) (ymax - 1) / 2
                        : y * (ymax - 1) / (itemsInLevel.Count - 1);
                    nodes.Add(GetNode(project.Name, xpos, ypos));
                    y = y + 1;
                }
            }
        }

        private void AddOrphans(IEnumerable<Project> orphans, ICollection<XElement> nodes, int ymax)
        {
            double y = 0;
            double x = -1;
            foreach (var project in orphans)
            {
                nodes.Add(GetNode(project.Name, x, y));
                y++;
                if (y < ymax)
                    continue;
                y = 0;
                x--;
            }
        }

        private XElement GetNode(string name, double x, double y)
        {
            return new XElement(_dgmlns + "Node",
                new XAttribute("Id", name),
                new XAttribute("Category", "Project"),
                new XAttribute("Label", name),
                new XAttribute("UseManualLocation", "True"),
                new XAttribute("Bounds", x * _options.CellWidth + "," + y * _options.CellHeight + ",120,45")
            );
        }

        private XElement GetLink(string source, string target, string category)
        {
            return new XElement(_dgmlns + "Link",
                new XAttribute("Source", source),
                new XAttribute("Target", target),
                new XAttribute("Category", category));
        }
    }
}