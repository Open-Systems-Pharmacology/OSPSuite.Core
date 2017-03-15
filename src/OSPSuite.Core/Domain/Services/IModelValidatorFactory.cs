namespace OSPSuite.Core.Domain.Services
{
   public interface IModelValidatorFactory
   {
      T Create<T>() where T : IModelValidator;
   }
}