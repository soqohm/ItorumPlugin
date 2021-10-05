using TFlex.Model;
using TFlex.Model.Model3D;

namespace Itorum
{
    public class QueueNode
    {
        public readonly Document doc;
        public readonly FNode node;

        public QueueNode(Document doc, FNode node)
        {
            this.doc = doc;
            this.node = node;
        }
    }

    public class FNode
    {
        public readonly Fragment3D f;
        public readonly int deep;
        public readonly FNode parent;

        public FNode(Fragment3D f, int deep, FNode parent)
        {
            this.f = f;
            this.deep = deep;
            this.parent = parent;
        }
    }
}
