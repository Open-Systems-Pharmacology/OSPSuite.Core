using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Adds the root name at the beginning of the path, if the path starts with one of the top container names provided as
   ///    parameters or if the path does not start with the root name already
   /// </summary>
   public class TopContainerPathReplacer : IKeywordInObjectPathReplacer
   {
      private readonly string _rootName;
      private readonly IReadOnlyList<string> _topContainerNames;

      public TopContainerPathReplacer(string rootName, IReadOnlyList<string> topContainerNames)
      {
         _rootName = rootName;
         _topContainerNames = topContainerNames;
      }

      public void ReplaceIn(ObjectPath objectPath)
      {
         //no element 
         if (!objectPath.Any())
            return;

         var firstPathElement = objectPath.ElementAt(0);

         if (string.Equals(firstPathElement, _rootName))
            return;

         if (!_topContainerNames.Contains(firstPathElement))
            return;

         objectPath.AddAtFront(_rootName);
      }
   }
}