using System.Collections.Generic;
using System.Linq;

using TFlex.Model.Model3D;

namespace Itorum
{
    public static class Linqs
    {
        public static IEnumerable<FNode> NoBroken(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Where(node => node.f.GetFragmentDocument(true, false) != null);
        }

        public static IEnumerable<FNode> NoLibrary(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Where(node => node.f.FilePath[0] != '<');
        }

        public static IEnumerable<FNode> Unique(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Distinct(new FNodeComparer());
        }

        public static IEnumerable<FNode> WithS(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Where(node => node.f.GetNumber().WithS());
        }

        public static IEnumerable<FNode> WithParent(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Select(node =>
                {
                    var rivet = new Rivet(node);
                    if (node.parent.f != null) rivet.Nodes.Add(node.parent);
                    return rivet;
                })
                .SelectMany(rivet => rivet.Nodes);
        }

        public static IEnumerable<FNode> EmptyName(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Where(fn => fn.f.GetVariableValue("$Наименование", false).TextValue == "");
        }

        public static IEnumerable<FNode> Part(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Where(fn => fn.f.GetFragmentDocument(true, false).GetFragments3D().Count == 0);
        }

        public static IEnumerable<FNode> Product(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Where(fn => fn.f.GetFragmentDocument(true, false).GetFragments3D().Count > 0);
        }

        public static IEnumerable<FNode> Adaptive(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Where(fn => fn.f.Associative);
        }

        public static IEnumerable<FNode> NameWithFLX(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Where(fn => fn.f.GetVariableValue("$Наименование", false).TextValue.Contains("FLX"));
        }

        public static IEnumerable<FNode> NameWithMCH(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Where(fn => fn.f.GetVariableValue("$Наименование", false).TextValue.Contains("MCH"));
        }

        public static IEnumerable<FNode> FileNameNoNumber(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Where(fn => fn.f.GetVariableValue("$Обозначение", false).TextValue != fn.f.FilePath);
        }

        public static IEnumerable<FNode> OnlyLibrary(this IEnumerable<FNode> nodes)
        {
            return nodes
                .Where(node => node.f.FilePath[0] == '<');
        }

        public class Rivet
        {
            public List<FNode> Nodes { get; set; }

            public Rivet(FNode node) 
            { 
                Nodes = new List<FNode>() { node }; 
            }
        }
    }
    public class FNodeComparer : IEqualityComparer<FNode>
    {
        public bool Equals(FNode fn1, FNode fn2)
        {
            return fn1.f.GetNumber() == fn2.f.GetNumber();
        }

        public int GetHashCode(FNode fn)
        {
            return fn.f.GetNumber().GetHashCode();
        }
    }
}
