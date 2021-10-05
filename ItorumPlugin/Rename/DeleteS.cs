using System.Collections.Generic;
using System.IO;
using System.Linq;

using TFlex.Model;
using TFlex.Model.Model3D;

namespace Itorum
{
    public class DeleteS
    {
        private readonly Document doc;
        private readonly string resultPath;
        private List<FNode> uniqueNodes;
        public List<FNode> OpeningNodes { get; set; }

        public DeleteS(Document doc)
        {
            this.doc = doc;
            resultPath = doc.GetResultFolder();
            GetSequences(doc
                .BreadthSearch()
                .NoBroken()
                .NoLibrary());
        }

        private void GetSequences(IEnumerable<FNode> nodes)
        {
            uniqueNodes = nodes
                .Unique()
                .ToList();
            OpeningNodes = nodes
                .WithS()
                .WithParent()
                .Unique()
                .OrderByDescending(fn => fn.deep)
                .ToList();
        }

        public void ReName(Document doc)
        {
            doc.BeginChanges("");
            foreach (var v in doc.GetVariables())
            {
                if (v.Name == "$Обозначение") v.TextValue = v.TextValue.DeletePrefix();
            }
            doc.EndChanges();
        }

        public void ReLink(Document doc)
        {
            doc.BeginChanges("");
            foreach (var f in doc.GetFragments3D())
            {
                f.FilePath = f.FilePath.Correct().DeletePrefix();
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
            foreach (var node in uniqueNodes)
            {
                File.Copy(node.f.FullFilePath, GetNewFPath(node));
            }
        }

        public string GetNewFPath(FNode node)
        {
            return resultPath + node.f.FilePath.Correct().DeletePrefix();
        }
    }
}
