using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

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
         if (importLogger.Status.Is(NotificationType.Error))
            throw new OSPSuiteException(importLogger.LogString);

         if (importLogger.Status.Is(NotificationType.Warning))
         {
            importLogger.Log.Each(Console.WriteLine);
         }


         return parameterValuesCache;
      }
   }
}