using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Populations
{
   public class Covariates
   {
      /// <summary>
      ///    List of user defined attributes for the individual (e.g. PopulationName, Genotype etc)
      /// </summary>
      public virtual Cache<string, string> Attributes { get; }

      public Covariates()
      {
         Attributes = new Cache<string, string>(onMissingKey: x => string.Empty);
      }

      public void AddCovariate<T>(string attributeName, T attributeValue)
      {
         Attributes[attributeName] = attributeValue.ToString();
      }

      public string Covariate(string covariateName)
      {
         return Attributes[covariateName];
      }
   }
}