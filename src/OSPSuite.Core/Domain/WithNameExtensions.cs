using System.Linq;
using static OSPSuite.Core.Domain.Constants.Parameters;

namespace OSPSuite.Core.Domain
{
   public static class WithNameExtensions 
   {
      public static bool HasGlobalExpressionName(this IWithName withName)
      {
         return AllGlobalRelExpParameters.Contains(withName.Name);
      }

      public static bool HasExpressionName(this IWithName parameter)
      {
         return HasGlobalExpressionName(parameter) || parameter.IsNamed(REL_EXP);
      }
   }
}