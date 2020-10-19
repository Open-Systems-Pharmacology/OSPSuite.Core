using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Extensions;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants.SensitivityAnalysisResults;

namespace OSPSuite.Core.Importer.Services
{
   public interface IPKParameterSensitivitiesImporter
   {
      IReadOnlyList<PKParameterSensitivity> ImportFrom(string fileFullPath, IModelCoreSimulation simulation, IImportLogger logger);
   }

   public class PKParameterSensitivitiesImporter : IPKParameterSensitivitiesImporter
   {
      private static readonly char[] ALLOWED_DELIMITERS = {',', ';', '\t'};

      public IReadOnlyList<PKParameterSensitivity> ImportFrom(string fileFullPath, IModelCoreSimulation simulation, IImportLogger logger)
      {
         try
         {
            foreach (var delimiter in ALLOWED_DELIMITERS)
            {
               var pkParameterSensitivities = pkParameterSensitivitiesFrom(fileFullPath, delimiter);
               //we found at least one individual, this is a valid file for the delimiter and we can exit
               if (pkParameterSensitivities.Any())
                  return pkParameterSensitivities;
            }

            //no match. Log 
            logger.AddError(Warning.SensitivityAnalysisFileFormatIsNotSupported);
            return Array.Empty<PKParameterSensitivity>();
         }
         catch (Exception e)
         {
            logger.AddError(e.FullMessage());
            return Array.Empty<PKParameterSensitivity>();
         }
      }

      private IReadOnlyList<PKParameterSensitivity> pkParameterSensitivitiesFrom(string fileFullPath, char delimiter)
      {
         using (var reader = new CsvReaderDisposer(fileFullPath, delimiter))
         {
            var pkParameterSensitivityList = new List<PKParameterSensitivity>();
            var csv = reader.Csv;
            var headers = csv.GetFieldHeaders();
            if (!headers.ContainsAll(new[] {PARAMETER, PK_PARAMETER, QUANTITY_PATH, VALUE}))
               return pkParameterSensitivityList;

            while (csv.ReadNextRecord())
            {
               var pkParameterSensitivity = new PKParameterSensitivity
               {
                  PKParameterName = csv[PK_PARAMETER],
                  ParameterName = csv[PARAMETER],
                  QuantityPath = csv[QUANTITY_PATH],
                  Value = csv.DoubleAt(VALUE)
               };
               pkParameterSensitivityList.Add(pkParameterSensitivity);
            }

            return pkParameterSensitivityList;
         }
      }
   }
}