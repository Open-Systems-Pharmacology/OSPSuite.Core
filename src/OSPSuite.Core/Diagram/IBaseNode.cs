using System.Drawing;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Diagram
{
   public interface IBaseNode : IBaseObject, IWithId, IWithName, IHasLayoutInfo
   {
      string Description { get; set; }
      void ShowParents();

      void AddLinkFrom(IBaseLink link);
      void AddLinkTo(IBaseLink link);

      void CopyLayoutInfoFrom(IBaseNode node, PointF parentLocation);
      IBaseNode Copy();
   }
}