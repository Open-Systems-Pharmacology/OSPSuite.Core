using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Helpers
{
   /// <summary>
   ///    Same as real configuration but does not clear cache so that we can test that cache is correct
   /// </summary>
   public class SimulationConfigurationForSpecs : SimulationConfiguration
   {
      public SimulationConfigurationForSpecs()
      {
         //Just create a default one for now
         Module = new Module();
      }

      public override void ClearCache()
      {
         /*do nothing*/
      }
   }
}