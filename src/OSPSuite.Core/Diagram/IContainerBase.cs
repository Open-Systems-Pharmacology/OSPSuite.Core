using System.Collections.Generic;
using System.Drawing;

namespace OSPSuite.Core.Diagram
{
   public interface IContainerBase : IWithLocation
   {
      IEnumerable<T> GetDirectChildren<T>() where T : class;
      IEnumerable<T> GetAllChildren<T>() where T : class;
      void AddChildNode(IBaseNode node);
      void RemoveChildNode(IBaseNode node);
      bool ContainsChildNode(IBaseNode node, bool recursive);

      RectangleF CalculateBounds();
      void SetHiddenRecursive(bool hidden);
      void PostLayoutStep();
      void Collapse(int level);
      void Expand(int level);
   }
}