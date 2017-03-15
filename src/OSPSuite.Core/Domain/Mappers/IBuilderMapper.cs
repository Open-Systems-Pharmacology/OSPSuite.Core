using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IBuilderMapper<in TInput, out TOutput>
   {
      TOutput MapFrom(TInput input, IBuildConfiguration buildConfiguration);
   }
}