using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.Services
{
   public class DisplayNameProvider : IDisplayNameProvider
   {
      public string DisplayNameFor(object objectToDisplay)
      {
         if (objectToDisplay == null)
            return string.Empty;

         var explicitFormula = objectToDisplay as ExplicitFormula;
         if (explicitFormula != null)
            return explicitFormula.Name;

         return objectToDisplay.ToString();
      }
   }
}