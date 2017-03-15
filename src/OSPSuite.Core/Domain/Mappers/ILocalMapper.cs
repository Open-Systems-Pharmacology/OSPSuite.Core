using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface ILocalMapper<TInput, TOutput, TLocal>
   {
      /// <summary>
      /// Maps from input to output using data provided by local.
      /// </summary>
      /// <param name="input">The input.</param>
      /// <param name="container">The local information.</param>
      /// <param name="buildConfiguration">The build configuration</param>
      TOutput MapFromLocal(TInput input, TLocal container,IBuildConfiguration buildConfiguration);
   }
}