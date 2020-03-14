using System.Data;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
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

      /// <summary>
      ///    Creates a <see cref="DataTable" /> containing the PK-Analyses of the populationSimulation.
      /// </summary>
      /// <remarks>The format of the table will be IndividualId, QuantityPath, ParameterName, Value, Unit</remarks>
      /// <param name="simulation">Simulation used to calculate the PK-Analyses</param>
      /// <param name="pkAnalyses">Actual pkAnalyses to convert to <see cref="DataTable" /></param>
      Task<DataTable> PKAnalysesToDataTableAsync(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation);

      /// <summary>
      ///    Creates a <see cref="DataTable" /> containing the PK-Analyses of the populationSimulation.
      /// </summary>
      /// <remarks>The format of the table will be IndividualId, QuantityPath, ParameterName, Value, Unit</remarks>
      /// <param name="simulation">Simulation used to calculate the PK-Analyses</param>
      /// <param name="pkAnalyses">Actual pkAnalyses to convert to <see cref="DataTable" /></param>
      DataTable PKAnalysesToDataTable(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation);

      DataTable SensitivityAnalysisResultsToDataTable(SensitivityAnalysisRunResult sensitivityAnalysisRunResult, IModelCoreSimulation simulation);
   }

   public class SimulationResultsToDataTableConverter : ISimulationResultsToDataTableConverter
   {
      private readonly IEntitiesInSimulationRetriever _quantityRetriever;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;
      private readonly IPKParameterRepository _pkParameterRepository;
      private readonly IDimension _timeDimension;

      public SimulationResultsToDataTableConverter(
         IDimensionFactory dimensionFactory, 
         IEntitiesInSimulationRetriever quantityRetriever, 
         IDisplayUnitRetriever displayUnitRetriever, 
         IPKParameterRepository pkParameterRepository)
      {
         _quantityRetriever = quantityRetriever;
         _displayUnitRetriever = displayUnitRetriever;
         _pkParameterRepository = pkParameterRepository;
         _timeDimension = dimensionFactory.Dimension(Constants.Dimension.TIME);
      }

      public Task<DataTable> ResultsToDataTableAsync(SimulationResults simulationResults, IModelCoreSimulation simulation)
      {
         return Task.Run(() => ResultsToDataTable(simulationResults, simulation));
      }

      public Task<DataTable> PKAnalysesToDataTableAsync(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation)
      {
         return Task.Run(() => PKAnalysesToDataTable(pkAnalyses, simulation));
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

      public DataTable PKAnalysesToDataTable(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation)
      {
         var dataTable = new DataTable(simulation.Name);

         dataTable.AddColumn<int>(INDIVIDUAL_ID);
         dataTable.AddColumn<string>(QUANTITY_PATH);
         dataTable.AddColumn<string>(PARAMETER);
         dataTable.AddColumn<string>(VALUE);
         dataTable.AddColumn<string>(UNIT);
         dataTable.AddColumn<string>(DISPLAY);

         dataTable.BeginLoadData();
         foreach (var pkAnalysis in pkAnalyses.All())
         {
            var unit = _displayUnitRetriever.PreferredUnitFor(pkAnalysis);
            var pkParameter = _pkParameterRepository.FindByName(pkAnalysis.Name);
            pkAnalysis.Values.Each((value, index) =>
            {
               var row = dataTable.NewRow();
               row[INDIVIDUAL_ID] = index;
               row[QUANTITY_PATH] = inQuote(pkAnalysis.QuantityPath); 
               row[PARAMETER] = inQuote(pkAnalysis.Name);
               row[VALUE] = pkAnalysis.ConvertToUnit(value, unit).ConvertedTo<string>();
               row[UNIT] = unit.Name;
               row[DISPLAY] = pkParameter.DisplayName;
               dataTable.Rows.Add(row);
            });
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

         dataTable.BeginLoadData();
         foreach (var pkParameterSensitivity in sensitivityAnalysisRunResult.AllPKParameterSensitivities)
         {
            var row = dataTable.NewRow();
            row[Constants.SensitivityAnalysisResults.QUANTITY_PATH] = inQuote(pkParameterSensitivity.QuantityPath);
            row[Constants.SensitivityAnalysisResults.PARAMETER] = inQuote(pkParameterSensitivity.ParameterName);
            row[Constants.SensitivityAnalysisResults.PK_PARAMETER] = inQuote(pkParameterSensitivity.PKParameterName);
            row[Constants.SensitivityAnalysisResults.VALUE] = pkParameterSensitivity.Value.ConvertedTo<string>();
            dataTable.Rows.Add(row);
         }

         dataTable.EndLoadData();
         return dataTable;
      }

      private string inQuote(string text)=> $"\"{text}\"";
   }
}