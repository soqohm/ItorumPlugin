using System.Collections.Generic;
using System.IO;
using System.Linq;

using TFlex.Model;
using TFlex.Model.Model3D;

namespace Itorum
{
    public class CatiaPart
    {
        private readonly Document doc;
        public readonly string resultPath;
        public readonly List<FNode> openingNodes;
        private readonly Dictionary<string, Part> parsing;
        public readonly string[] emptyName;

        public CatiaPart(Document doc)
        {
            this.doc = doc;
            resultPath = doc.GetResultFolder();
            openingNodes = doc
                .BreadthSearch()
                .Unique()
                .OrderByDescending(fn => fn.deep)
                .ToList();
            parsing = openingNodes
                .ToDictionary(
                fn => fn.f.FilePath,
                fn => new Part(fn.f));
            emptyName = openingNodes
                .EmptyName()
                .Select(fn => fn.f.GetNumber())
                .ToArray();
        }

        public void ReName(Document doc, FNode node)
        {
            var part = parsing[node.f.FilePath];
            doc.BeginChanges("");
            foreach (var v in doc.GetVariables())
            {
                if (v.Name == "$Обозначение") v.TextValue = part.Number;
                if (v.Name == "$Наименование") v.TextValue = part.Name;
                if (v.Name == "$Vid_Chert") v.TextValue = part.Section;
            }
            doc.EndChanges();
        }

        public void ReLink(Document doc)
        {
            doc.BeginChanges("");
            foreach (var f in doc.GetFragments3D())
            {
                var key = f.FilePath;
                if (parsing.ContainsKey(key))
                    f.FilePath = parsing[key].FileName;
            }
            doc.EndChanges();
        }

        public void EditFirstLevel()
        {
            ReLink(doc);
            doc.SaveAs(resultPath + doc.FileName.Correct());
            doc.Regenerate(new RegenerateOptions()
            {
                UpdateAllLinks = true,
                UpdateProductStructures = true
            });
        }

        public void ReSaveNodes()
        {
            Directory.CreateDirectory(resultPath);
            foreach (var node in openingNodes)
            {
                File.Copy(node.f.FullFilePath, GetNewFPath(node));
            }
        }

        public string GetNewFPath(FNode node)
        {
            return resultPath + parsing[node.f.FilePath].FileName;
        }

        public class Part
        {
            public string Number { get; set; }
            public string Name { get; set; }
            public string FileName { get; set; }
            public string Section { get; set; }

            public Part(Fragment3D f)
            {
                GetNumber(f);
                GetName(f);
                GetFileName();
                GetSection(f);
            }

            private void GetNumber(Fragment3D f)
            {
                Number = f.GetNumber();
                if (Number.Contains(" _ "))
                    Number = Number
                    .SplitS()[0]
                    .DeleteRevision()
                    .DeletePrefix();
                else
                    Number = Number
                    .DeleteBrackets()
                    .DeleteRevision()
                    .DeletePrefix();
            }

            private void GetName(Fragment3D f)
            {
                Name = f.GetName();
                if (Name.Contains(" _ "))
                    Name = Name
                    .SplitS()[1]
                    .DeleteBrackets();
                else Name = "";
            }

            private void GetFileName()
            {
                FileName = Number.Replace('/', '-') + ".grb";
            }

            private void GetSection(Fragment3D f)
            {
                if (f.IsProduct()) Section = "Сборочный чертеж";
                else Section = "Чертеж";
            }
        }
    }
}
