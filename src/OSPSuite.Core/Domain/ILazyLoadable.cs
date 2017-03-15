namespace OSPSuite.Core.Domain
{
   public interface ILazyLoadable
   {
      /// <summary>
      ///    Indicates if the object is fully loaded
      /// </summary>
      bool IsLoaded { get; set; }
   }
}