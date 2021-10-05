using System;
using System.Collections.Generic;
using System.IO;

using TFlex.Model;
using TFlex.Model.Model3D;

namespace Itorum
{
    public static class DocumentExtensions
    {
        public static IEnumerable<FNode> BreadthSearch(this Document document)
        {
            var queue = new Queue<QueueNode>();
            queue.Enqueue(new QueueNode(document, new FNode(null, 0, null)));
            while (queue.Count != 0)
            {
                var queueNode = queue.Dequeue();
                var deep = queueNode.node.deep + 1;
                var parent = queueNode.node;
                foreach (var f in queueNode.doc.GetFragments3D())
                {
                    var fDoc = f.GetFragmentDocument(true, false);
                    if (fDoc.IsNotNull() && fDoc.IsProduct())
                    {
                        queue.Enqueue(new QueueNode(fDoc, new FNode(f, deep, parent)));
                        yield return new FNode(f, deep, parent);
                    }
                    else yield return new FNode(f, 100, parent);
                }
            }
        }

        public static bool IsNotNull(this Document doc)
        {
            return doc != null;
        }

        public static bool IsProduct(this Document doc)
        {
            return doc.GetFragments3D().Count > 0;
        }

        public static Document OpenDoc(string path) 
        { 
            return TFlex.Application.OpenDocument(path, false); 
        }

        public static string GetNumber(this Document doc) 
        { 
            return doc.FindVariable("$Обозначение").TextValue; 
        }

        public static string GetResultFolder(this Document doc)
        {
            var path = doc.FilePath + "!RENAME";
            if (!Directory.Exists(path)) return path + @"\";
            else
            {
                var i = 1;
                while (Directory.Exists(path + " (" + i + ")")) i++;
                return path + " (" + i + ")" + @"\";
            }
        }

        public static void CatiaRename(this Document document)
        {
            var catia = new CatiaPart(document);
            catia.ReSaveNodes();
            foreach (var node in catia.openingNodes)
            {
                var doc = OpenDoc(catia.GetNewFPath(node));
                catia.ReName(doc, node);
                catia.ReLink(doc);
                doc.Save();
                doc.Close();
            }
            catia.EditFirstLevel();
            var path = catia.resultPath + "! детали без наименования.log";
            catia.emptyName.WriteLog(path);
            Logs.OpenLog(path);
        }

        public static void DeleteS(this Document document)
        {
            var deleteS = new DeleteS(document);
            deleteS.ReSaveNodes();
            foreach (var node in deleteS.OpeningNodes)
            {
                var doc = OpenDoc(deleteS.GetNewFPath(node));
                deleteS.ReName(doc);
                deleteS.ReLink(doc);
                doc.Regenerate(new RegenerateOptions() { UpdateAllLinks = true });
                doc.Save();
                doc.Close();
            }
            deleteS.EditFirstLevel();
        }

        public static void EditTopLevel(this Document doc, Action<Fragment3D> action)
        {
            doc.BeginChanges("");
            foreach (var f in doc.GetFragments3D())
            {
                action(f);
            }
            doc.EndChanges();
            doc.Regenerate(new RegenerateOptions() { UpdateAllLinks = true });
        }

        public static void WithElements(this Document doc)
        {
            EditTopLevel(doc, f => f.IncludeInNewBom = IncludeInBom.WithEmbeddedElements);
        }

        public static void LinkCorrection(this Document doc)
        {
            EditTopLevel(doc, f => f.FilePath = f.FilePath.Correct());
        }
    }
}