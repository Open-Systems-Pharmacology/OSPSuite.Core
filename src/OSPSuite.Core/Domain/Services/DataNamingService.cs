namespace OSPSuite.Core.Domain.Services
{
   public interface IDataNamingService
   {
      string GetTimeName();

      /// <summary>
      /// Name for the new DataRepository
      /// </summary>
      /// <returns>Name for the new DataRepository</returns>
      string GetNewRepositoryName();

      /// <summary>
      /// Gets name for given Entity Id
      /// </summary>
      /// <param name="id"></param>
      string GetEntityName(string id);
   }
}