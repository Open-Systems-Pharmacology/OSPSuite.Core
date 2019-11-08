using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services
{
   public interface IModelValidatorFactory
   {
      T Create<T>() where T : IModelValidator;
   }

   internal class ModelValidatorFactory : DynamicFactory<IModelValidator>, IModelValidatorFactory
   {
   }
}