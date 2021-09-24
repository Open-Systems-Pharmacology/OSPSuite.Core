using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationRunInitializerFactory
   {
      T Create<T>() where T : IParameterIdentificationRunInitializer;
   }

   class ParameterIdentificationRunInitializerFactory : DynamicFactory<IParameterIdentificationRunInitializer>, IParameterIdentificationRunInitializerFactory
   {
      public ParameterIdentificationRunInitializerFactory(Utility.Container.IContainer container) : base(container)
      {
      }
   }
}