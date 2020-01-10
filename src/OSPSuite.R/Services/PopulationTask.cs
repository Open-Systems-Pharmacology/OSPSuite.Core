using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.R.Extensions;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Services
{
   public interface IPopulationTask
   {
      IndividualValuesCache ImportPopulation(string fileFullPath);

      DataTable PopulationTableFrom(IndividualValuesCache population, IModelCoreSimulation simulation = null);

      /// <summary>
      /// Loads the population from the <paramref name="populationFile"/> and split the loaded population according to the <paramref name="numberOfCores"/>.
      /// Resulting files will be exported in the <paramref name="outputFolder"/>. File names will be constructed using the <paramref name="outputFileName"/> concatenated with the node index.
      /// Returns an array of string containing the full path of the population files created
      /// </summary>
      IReadOnlyList<string> SplitPopulation(string populationFile, int numberOfCores, string outputFolder, string outputFileName);
   }

   public class PopulationTask : IPopulationTask
   {
      private readonly IIndividualValuesCacheImporter _individualValuesCacheImporter;
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;

      public PopulationTask(IIndividualValuesCacheImporter individualValuesCacheImporter, IEntitiesInSimulationRetriever entitiesInSimulationRetriever)
      {
         _individualValuesCacheImporter = individualValuesCacheImporter;
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
      }

      public IndividualValuesCache ImportPopulation(string fileFullPath)
      {
         var importLogger = new ImportLogger();
         var parameterValuesCache = _individualValuesCacheImporter.ImportFrom(fileFullPath, importLogger);
         importLogger.LogToR();
         return parameterValuesCache;
      }

      public DataTable PopulationTableFrom(IndividualValuesCache population, IModelCoreSimulation simulation = null)
      {
         var dataTable = new DataTable();
         var allParameters = _entitiesInSimulationRetriever.QuantitiesFrom(simulation);
         dataTable.BeginLoadData();

         //Create one column for the parameter path
         addCovariates(population, dataTable);

         //add advanced parameters
         foreach (var parameterValues in population.AllParameterValues)
         {
            var parameterPathWithoutUnit = allParameters.Contains(parameterValues.ParameterPath) ? parameterValues.ParameterPath : parameterValues.ParameterPath.StripUnit();
            addColumnValues(dataTable, parameterPathWithoutUnit, parameterValues.Values);
         }

         dataTable.EndLoadData();
         return dataTable;
      }

      public IReadOnlyList<string> SplitPopulation(string populationFile, int numberOfCores, string outputFolder, string outputFileName)
      {
         var population = ImportPopulation(populationFile);
         var populationData = PopulationTableFrom(population);
         var dataSplitter = new PopulationDataSplitter(numberOfCores, populationData);
         DirectoryHelper.CreateDirectory(outputFolder);
         var outputFiles = new List<string>();

         for (int i = 0; i < numberOfCores; i++)
         {
            var outputFile = Path.Combine(outputFolder, $"{outputFileName}_{i + 1}{Constants.Filter.CSV_EXTENSION}");
            var rowIndices = dataSplitter.GetRowIndices(i).ToList();

            //This is potentially empty if the number of individuals in the population is less than the number of cores provided
            if(!rowIndices.Any())
               continue;
            
            outputFiles.Add(outputFile);
            exportSplitPopulation(populationData, rowIndices, outputFile);
         }

         return outputFiles;
      }

      private void exportSplitPopulation(DataTable populationData, IReadOnlyList<int> rowIndices, string outputFile)
      {
         var dataTable = populationData.Clone();
         rowIndices.Each(index =>
         {
            dataTable.ImportRow(populationData.Rows[index]);
         });
         dataTable.ExportToCSV(outputFile);
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