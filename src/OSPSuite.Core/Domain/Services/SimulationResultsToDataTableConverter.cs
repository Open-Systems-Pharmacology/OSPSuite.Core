using System.Data;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants.SimulationResults;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimulationResultsToDataTableConverter
   {
      /// <summary>
      ///    Creates a <see cref="DataTable" /> containing  the results of the simulation.
      /// </summary>
      /// <remarks>The format of the table will be one column per output and one row per individual</remarks>
      /// <param name="simulation">Simulation used to create the results</param>
      /// <param name="simulationResults">Actual results to convert to <see cref="DataTable" /></param>
      Task<DataTable> ResultsToDataTableAsync(SimulationResults simulationResults, IModelCoreSimulation simulation);

      /// <summary>
      ///    Creates a <see cref="DataTable" /> containing  the results of the simulation.
      /// </summary>
      /// <remarks>The format of the table will be one column per output and one row per individual</remarks>
      /// <param name="simulation">Simulation used to create the results</param>
      /// <param name="simulationResults">Actual results to convert to <see cref="DataTable" /></param>
      DataTable ResultsToDataTable(SimulationResults simulationResults, IModelCoreSimulation simulation);


      DataTable SensitivityAnalysisResultsToDataTable(SensitivityAnalysisRunResult sensitivityAnalysisRunResult, IModelCoreSimulation simulation);
   }

   public class SimulationResultsToDataTableConverter : ISimulationResultsToDataTableConverter
   {
      private readonly IEntitiesInSimulationRetriever _quantityRetriever;
      private readonly IDimension _timeDimension;

      public SimulationResultsToDataTableConverter(
         IDimensionFactory dimensionFactory, 
         IEntitiesInSimulationRetriever quantityRetriever
         )
      {
         _quantityRetriever = quantityRetriever;
         _timeDimension = dimensionFactory.Dimension(Constants.Dimension.TIME);
      }

      public Task<DataTable> ResultsToDataTableAsync(SimulationResults simulationResults, IModelCoreSimulation simulation)
      {
         return Task.Run(() => ResultsToDataTable(simulationResults, simulation));
      }

      public DataTable ResultsToDataTable(SimulationResults simulationResults, IModelCoreSimulation simulation)
      {
         //Id	Time	Output1	Output2	...	OutputN
         var dataTable = new DataTable();

         var allQuantities = _quantityRetriever.QuantitiesFrom(simulation);
         var timeColumnName = Constants.NameWithUnitFor(TIME, _timeDimension.BaseUnit);
         var quantityPathCache = new Cache<string, string>();
         dataTable.AddColumn<int>(INDIVIDUAL_ID);
         dataTable.AddColumn<string>(timeColumnName);

         if (simulationResults.IsNull())
            return dataTable;

         var allQuantityPaths = simulationResults.AllQuantityPaths();
         foreach (var quantityPath in allQuantityPaths)
         {
            var quantity = allQuantities[quantityPath];
            if (quantity == null) continue;

            //export results in base unit so that they can be computed automatically from matlab scripts
            quantityPathCache[quantityPath] = Constants.NameWithUnitFor(quantityPath, quantity.Dimension.BaseUnit);
            dataTable.AddColumn<string>(quantityPathCache[quantityPath]);
         }

         dataTable.BeginLoadData();
         int numberOfValues = simulationResults.Time.Length;

         foreach (var individualResults in simulationResults.OrderBy(x => x.IndividualId))
         {
            var allQuantitiesCache = new Cache<string, QuantityValues>(x => x.QuantityPath);
            allQuantitiesCache.AddRange(individualResults);

            for (int i = 0; i < numberOfValues; i++)
            {
               var row = dataTable.NewRow();
               row[INDIVIDUAL_ID] = individualResults.IndividualId;
               row[timeColumnName] = simulationResults.Time[i].ConvertedTo<string>();

               foreach (var quantityPath in allQuantityPaths)
               {
                  var quantity = allQuantities[quantityPath];
                  if (quantity == null) continue;

                  row[quantityPathCache[quantityPath]] = allQuantitiesCache[quantityPath][i].ConvertedTo<string>();
               }

               dataTable.Rows.Add(row);
            }
         }

         dataTable.EndLoadData();
         return dataTable;
      }


      public DataTable SensitivityAnalysisResultsToDataTable(SensitivityAnalysisRunResult sensitivityAnalysisRunResult, IModelCoreSimulation simulation)
      {
         var dataTable = new DataTable(simulation.Name);

         dataTable.AddColumn<string>(Constants.SensitivityAnalysisResults.QUANTITY_PATH);
         dataTable.AddColumn<string>(Constants.SensitivityAnalysisResults.PARAMETER);
         dataTable.AddColumn<string>(Constants.SensitivityAnalysisResults.PK_PARAMETER);
         dataTable.AddColumn<string>(Constants.SensitivityAnalysisResults.VALUE);
         dataTable.AddColumn<string>(Constants.SensitivityAnalysisResults.PARAMETER_PATH);

         dataTable.BeginLoadData();
         foreach (var pkParameterSensitivity in sensitivityAnalysisRunResult.AllPKParameterSensitivities)
         {
            var row = dataTable.NewRow();
            row[Constants.SensitivityAnalysisResults.QUANTITY_PATH] = pkParameterSensitivity.QuantityPath.InQuotes();
            row[Constants.SensitivityAnalysisResults.PARAMETER] = pkParameterSensitivity.ParameterName.InQuotes();
            row[Constants.SensitivityAnalysisResults.PK_PARAMETER] = pkParameterSensitivity.PKParameterName.InQuotes();
            row[Constants.SensitivityAnalysisResults.VALUE] = pkParameterSensitivity.Value.ConvertedTo<string>();
            row[Constants.SensitivityAnalysisResults.PARAMETER_PATH] = pkParameterSensitivity.ParameterPath.InQuotes();
            dataTable.Rows.Add(row);
         }

         dataTable.EndLoadData();
         return dataTable;
      }
   }
}