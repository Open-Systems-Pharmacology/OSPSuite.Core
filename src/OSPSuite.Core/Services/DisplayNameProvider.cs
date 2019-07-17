using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Services
{
   public class DisplayNameProvider : IDisplayNameProvider
   {
      public string DisplayNameFor(object objectToDisplay)
      {
         if (objectToDisplay == null)
            return string.Empty;

         if (objectToDisplay is ExplicitFormula explicitFormula)
            return explicitFormula.Name;

         return objectToDisplay.ToString();
      }
   }
}