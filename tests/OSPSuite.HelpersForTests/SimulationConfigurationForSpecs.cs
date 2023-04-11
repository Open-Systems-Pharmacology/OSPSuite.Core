using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Helpers
{
   /// <summary>
   ///    Same as real configuration but does not clear cache so that we can test that cache is correct
   /// </summary>
   public class SimulationConfigurationForSpecs : SimulationConfiguration
   {
      public override void ClearCache()
      {
         /*do nothing*/
      }
   }
}