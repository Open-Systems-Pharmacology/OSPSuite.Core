using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationRunInitializerFactory
   {
      T Create<T>() where T : IParameterIdentifcationRunInitializer;
   }

   class ParameterIdentificationRunInitializerFactory : DynamicFactory<IParameterIdentifcationRunInitializer>, IParameterIdentificationRunInitializerFactory
   {
   }
}