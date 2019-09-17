using OSPSuite.Core.Domain.Populations;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.R.Extensions;

namespace OSPSuite.R.Services
{
   public interface IPopulationImporter
   {
      IndividualValuesCache ImportPopulation(string fileFullPath);
   }

   public class PopulationImporter : IPopulationImporter
   {
      private readonly IIndividualValuesCacheImporter _individualValuesCacheImporter;

      public PopulationImporter(IIndividualValuesCacheImporter individualValuesCacheImporter)
      {
         _individualValuesCacheImporter = individualValuesCacheImporter;
      }

      public IndividualValuesCache ImportPopulation(string fileFullPath)
      {
         var importLogger = new ImportLogger();
         var parameterValuesCache = _individualValuesCacheImporter.ImportFrom(fileFullPath, importLogger);
         importLogger.LogToR();
         return parameterValuesCache;
      }
   }
}