namespace OSPSuite.Core.Domain.Services
{
   public interface ILazyLoadTask
   {

      /// <summary>
      ///    Loads the given object from the project database (if not already loaded)
      /// </summary>
      /// <typeparam name="TObject">Type of object to load</typeparam>
      /// <param name="objectToLoad">Lazy object to load</param>
      void Load<TObject>(TObject objectToLoad) where TObject : class, ILazyLoadable;
   }
}