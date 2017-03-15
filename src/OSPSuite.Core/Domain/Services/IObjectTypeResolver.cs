namespace OSPSuite.Core.Domain.Services
{
   public interface IObjectTypeResolver
   {
      string TypeFor<T>(T objectRequiringType) where T : class;
      string TypeFor<T>();
   }
}