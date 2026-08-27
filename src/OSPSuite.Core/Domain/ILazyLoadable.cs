namespace OSPSuite.Core.Domain
{
   public interface ILazyLoadable
   {
      /// <summary>
      ///    Indicates if the object is fully loaded. Lazy loading reads this flag without holding a lock to skip objects
      ///    that are already loaded, so an implementation must publish it safely, for example with a volatile backing
      ///    field. Otherwise a reader can observe the flag before the writes that loaded the object.
      /// </summary>
      bool IsLoaded { get; set; }
   }
}