using System.Linq;
using static OSPSuite.Core.Domain.Constants.Parameters;

namespace OSPSuite.Core.Domain
{
   public static class WithNameExtensions 
   {
      public static bool HasGlobalExpressionName(this IWithName withName)
      {
         return withName != null && AllGlobalRelExpParameters.Contains(withName.Name);
      }

      public static bool HasExpressionName(this IWithName withName)
      {
         return withName != null && (HasGlobalExpressionName(withName) || withName.IsNamed(REL_EXP));
      }
   }
}