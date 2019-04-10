using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Qualification
{
   public static class QualificationExtensions
   {
      public static T[] ForProject<T>(this IEnumerable<T> referencingProject, string projectName) where T : IReferencingProject =>
         referencingProject?.Where(x => string.Equals(x.Project, projectName)).ToArray();
   }
}