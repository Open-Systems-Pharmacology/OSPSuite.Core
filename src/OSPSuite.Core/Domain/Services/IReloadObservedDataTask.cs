namespace OSPSuite.Core.Domain.Services
{
   public interface IReloadObservedDataTask
   {
      /// <summary>
      ///    Reloads all observed data in the project that was imported with the importer configuration
      ///    <paramref name="configurationId" />
      /// </summary>
      void ReloadAllObservedDataImportedWith(string configurationId);
   }
}
