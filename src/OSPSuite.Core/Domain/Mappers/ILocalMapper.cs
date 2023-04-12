using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface ILocalMapper<in TInput, in TLocal, out TOutput>
   {
      /// <summary>
      ///    Maps from input to output using data provided by local.
      /// </summary>
      /// <param name="input">The input.</param>
      /// <param name="container">The local information.</param>
      /// <param name="simulationBuilder">The build configuration</param>
      TOutput MapFromLocal(TInput input, TLocal container, SimulationBuilder simulationBuilder);
   }
}