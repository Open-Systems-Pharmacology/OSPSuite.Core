using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain
{
   public class UsedObservedData
   {
      /// <summary>
      ///    Id of the data repository containing the observed data
      /// </summary>
      public string Id { get; set; }

      /// <summary>
      ///    Reference to <see cref="ISimulation" /> referencing this <see cref="UsedObservedData" />
      /// </summary>
      public ISimulation Simulation { get; set; }

      public static UsedObservedData From(DataRepository dataRepository)
      {
         return new UsedObservedData {Id = dataRepository.Id};
      }

      public UsedObservedData Clone()
      {
         return new UsedObservedData {Id = Id};
      }
   }
}