using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Add the modelname at the begining of the path, if the path starts with one of the top container names
   /// </summary>
   public class TopContainerPathReplacer : IKeywordInObjectPathReplacer
   {
      private readonly string _rootName;
      private readonly IEnumerable<string> _topContainerNames;

      public TopContainerPathReplacer(string rootName, IEnumerable<string> topContainerNames)
      {
         _rootName = rootName;
         _topContainerNames = topContainerNames;
      }

      public void ReplaceIn(IObjectPath objectPath)
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