using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.R.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Services
{
   public interface IPopulationTask
   {
      IndividualValuesCache ImportPopulation(string fileFullPath);
      DataTable PopulationTableFrom(IndividualValuesCache population);
   }

   public class PopulationTask : IPopulationTask
   {
      private readonly IIndividualValuesCacheImporter _individualValuesCacheImporter;

      public PopulationTask(IIndividualValuesCacheImporter individualValuesCacheImporter)
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

      public DataTable PopulationTableFrom(IndividualValuesCache population)
      {
         var dataTable = new DataTable();
         dataTable.BeginLoadData();

         //Create one column for the parameter path
         addCovariates(population, dataTable);

         //add advanced parameters
         foreach (var parameterValues in population.AllParameterValues)
         {
            addColumnValues(dataTable, parameterValues.ParameterPath, parameterValues.Values);
         }

         dataTable.EndLoadData();
         return dataTable;
      }

      private void addCovariates(IndividualValuesCache population, DataTable dataTable)
      {
         var individualIds = Enumerable.Range(0, population.Count).ToList();

         //add individual ids column
         individualIds.Each(i => dataTable.Rows.Add(dataTable.NewRow()));
         addColumnValues(dataTable, Constants.Population.INDIVIDUAL_ID_COLUMN, individualIds);

         //and one column for each individual in the population
         foreach (var covariateName in population.AllCovariatesNames())
         {
            addColumnValues(dataTable, covariateName, population.AllCovariateValuesFor(covariateName));
         }
      }

      private void addColumnValues<T>(DataTable dataTable, string columnName, IReadOnlyList<T> allValues)
      {
         dataTable.AddColumn<T>(columnName);
         for (int i = 0; i < allValues.Count; i++)
         {
            dataTable.Rows[i][columnName] = allValues[i];
         }
      }
   }
}