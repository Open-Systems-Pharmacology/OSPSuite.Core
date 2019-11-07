using System;
using System.Collections.Generic;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import.Services
{
   public interface IIndividualValuesCacheImporter
   {
      /// <summary>
      ///    Imports all parameters defined in the <paramref name="populationFileFullPath" />. If
      ///    <paramref name="allParameters" /> is defined,
      ///    the imported parameter path will be validated (unit removed, check for existence). Otherwise, parameters will be
      ///    imported as is
      /// </summary>
      /// <param name="populationFileFullPath">Full path of CSV File containing the population to import</param>
      /// <param name="logger">Logger used to notify user of potential problem with the file</param>
      /// <param name="allParameters">Optional cache of parameters used to validate the imported parameters</param>
      /// <returns></returns>
      IndividualValuesCache ImportFrom(string populationFileFullPath, IImportLogger logger, PathCache<IParameter> allParameters = null);
   }

   public class IndividualValuesCacheImporter : IIndividualValuesCacheImporter
   {
      private static readonly char[] ALLOWED_DELIMITERS = {',', ';', '\t'};

      public IndividualValuesCache ImportFrom(string populationFileFullPath, IImportLogger logger, PathCache<IParameter> allParameters = null)
      {
         try
         {
            foreach (var delimiter in ALLOWED_DELIMITERS)
            {
               var individualValuesCache = individualValuesCacheFrom(populationFileFullPath, delimiter);
               //we found at least one individual, this is a valid file for the delimiter and we can exit
               if (individualValuesCache?.Count > 0)
                  return withPathsContainingUnitsUpdated(individualValuesCache, allParameters, logger);
            }

            //no match. Log 
            logger.AddError(Warning.PopulationFileFormatIsNotSupported);
            return new IndividualValuesCache();
         }
         catch (Exception e)
         {
            logger.AddError(e.FullMessage());
            return new IndividualValuesCache();
         }
      }

      private IndividualValuesCache withPathsContainingUnitsUpdated(IndividualValuesCache individualValuesCache, PathCache<IParameter> allParameters, IImportLogger logger)
      {
         // No parameters to check from, return as IS
         if (allParameters == null)
            return individualValuesCache;

         //Use TO LIST here because collection might be modified
         foreach (var parameterValue in individualValuesCache.AllParameterValues.ToList())
         {
            var parameterPath = parameterValue.ParameterPath;
            if (allParameters.Contains(parameterPath))
               continue;

            var pathWithUnitsRemoved = parameterPath.StripUnit();
            if (allParameters.Contains(pathWithUnitsRemoved))
            {
               individualValuesCache.RenamePath(parameterPath, pathWithUnitsRemoved);
               parameterValue.ParameterPath = pathWithUnitsRemoved;
               continue;
            }

            logger.AddWarning(Warning.ParameterWithPathNotFoundInBaseIndividual(parameterPath));
            individualValuesCache.Remove(parameterPath);
         }

         return individualValuesCache;
      }

      private IndividualValuesCache individualValuesCacheFrom(string fileFullPath, char delimiter)
      {
         using (var reader = new CsvReaderDisposer(fileFullPath, delimiter))
         {
            var csv = reader.Csv;
            var headers = csv.GetFieldHeaders();
            if (headers.Contains(Constants.Population.INDIVIDUAL_ID_COLUMN))
               return createIndividualPropertiesFromCSV(csv, headers);
         }

         return null;
      }

      private IndividualValuesCache createIndividualPropertiesFromCSV(CsvReader csv, string[] headers)
      {
         var individualPropertiesCache = new IndividualValuesCache();

         //first create a cache of all possible values
         var covariateCache = new Cache<string, List<string>>();
         var parameterValues = new Cache<string, List<double>>();
         int fieldCount = csv.FieldCount;
         int indexIndividualId = 0;

         for (int i = 0; i < headers.Length; i++)
         {
            var header = headers[i];
            if (string.Equals(header, Constants.Population.INDIVIDUAL_ID_COLUMN))
            {
               indexIndividualId = i;
               continue;
            }

            if (entryRepresentsParameter(header))
               parameterValues[header] = new List<double>();
            else
               covariateCache[header] = new List<string>();
         }

         while (csv.ReadNextRecord())
         {
            for (int i = 0; i < fieldCount; i++)
            {
               if (i == indexIndividualId)
                  continue;

               var header = headers[i];
               if (parameterValues.Contains(header))
                  parameterValues[header].Add(csv.DoubleAt(i));
               else
                  covariateCache[header].Add(csv[i]);
            }
         }

         //now fill the property cache
         addCovariates(individualPropertiesCache, covariateCache);

         foreach (var parameterValue in parameterValues.KeyValues)
         {
            individualPropertiesCache.SetValues(parameterValue.Key, parameterValue.Value);
         }

         return individualPropertiesCache;
      }

      private void addCovariates(IndividualValuesCache individualPropertiesCache, Cache<string, List<string>> covariateCache)
      {
         foreach (var covariate in covariateCache.KeyValues)
         {
            individualPropertiesCache.AddCovariate(covariate.Key, covariate.Value);
         }
      }

      private bool entryRepresentsParameter(string parameterPath)
      {
         if (parameterPath.IsOneOf(Constants.Population.RACE_INDEX, Constants.Population.GENDER, Constants.Population.POPULATION))
            return false;

         return parameterPath.Contains(ObjectPath.PATH_DELIMITER);
      }
   }
}