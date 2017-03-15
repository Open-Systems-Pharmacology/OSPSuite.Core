using System.Drawing;

namespace OSPSuite.Core.Diagram
{
   public interface IElementBaseNode : IBaseNode
   {
      SizeF NodeBaseSize { get; set; }
      NodeSize NodeSize { get; set; }
      bool CanLink { get; set; }
      T FindChild<T>(string childName);
   }
}