using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   internal class NodeCollection : IEnumerable<IBaseNode>
   {
      private readonly List<IBaseNode> _nodes = new List<IBaseNode>();

      public int Count => _nodes.Count;

      public void Add(IBaseNode node)
      {
         if (_nodes.Contains(node)) return;
         _nodes.Add(node);
      }

      public bool Remove(IBaseNode node) => _nodes.Remove(node);

      public bool Contains(IBaseNode node) => _nodes.Contains(node);

      public void Clear() => _nodes.Clear();

      public void MoveToFront(IBaseNode node)
      {
         if (_nodes.Remove(node))
            _nodes.Add(node);
      }

      public void MoveToBack(IBaseNode node)
      {
         if (_nodes.Remove(node))
            _nodes.Insert(0, node);
      }

      public IEnumerator<IBaseNode> GetEnumerator() => _nodes.ToList().GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
   }
}
