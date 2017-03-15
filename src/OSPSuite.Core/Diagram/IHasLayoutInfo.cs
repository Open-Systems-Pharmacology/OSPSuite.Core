using System.Collections.Generic;

namespace OSPSuite.Core.Diagram
{
   public interface IHasLayoutInfo : IWithVisible, IWithLocation, IWithColor
   {
      bool Hidden { get; set; }
      bool IsVisible { get; set; }
      bool LocationFixed { get; set; }
      void ToFront();
      void ToBack();
      int UserFlags { get; set; }
      IEnumerable<T> GetLinkedNodes<T>() where T : class, IHasLayoutInfo;
   }
}