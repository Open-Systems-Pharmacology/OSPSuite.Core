using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services
{
   internal interface IModelValidatorFactory
   {
      T Create<T>() where T : IModelValidator;
   }

   internal class ModelValidatorFactory : DynamicFactory<IModelValidator>, IModelValidatorFactory
   {
      public ModelValidatorFactory(Utility.Container.IContainer container) : base(container)
      {
      }
   }
}