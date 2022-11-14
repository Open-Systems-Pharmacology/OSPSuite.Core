using System.Data;
using System.Threading.Tasks;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants.SimulationResults;

namespace OSPSuite.Core.Domain.Services
{
   public interface IPopulationSimulationPKAnalysesToDataTableConverter
   {
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
   }

   public class PopulationSimulationPKAnalysesToDataTableConverter : IPopulationSimulationPKAnalysesToDataTableConverter
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IPKParameterRepository _pkParameterRepository;

      public PopulationSimulationPKAnalysesToDataTableConverter(
         IDimensionFactory dimensionFactory, 
         IPKParameterRepository pkParameterRepository)
      {
         _dimensionFactory = dimensionFactory;
         _pkParameterRepository = pkParameterRepository;
      }

      public Task<DataTable> PKAnalysesToDataTableAsync(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation)
      {
         return Task.Run(() => PKAnalysesToDataTable(pkAnalyses, simulation));
      }

      public DataTable PKAnalysesToDataTable(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation)
      {
         var dataTable = new DataTable(simulation.Name);

         dataTable.AddColumn<int>(INDIVIDUAL_ID);
         dataTable.AddColumn<string>(QUANTITY_PATH);
         dataTable.AddColumn<string>(PARAMETER);
         dataTable.AddColumn<string>(VALUE);
         dataTable.AddColumn<string>(UNIT);

         dataTable.BeginLoadData();
         foreach (var quantityPKParameter in pkAnalyses.All())
         {
            var quantityPath = quantityPKParameter.QuantityPath;
            var molWeight = simulation.MolWeightFor(quantityPath);
            var pkParameter = _pkParameterRepository.FindByName(quantityPKParameter.Name);
            var quantityPKParameterContext = new QuantityPKParameterContext(quantityPKParameter, molWeight);
            var mergedDimension = _dimensionFactory.MergedDimensionFor(quantityPKParameterContext);

            // In case the pkParameter is not found in the PkParameterRepository, use the mergedDimension default
            // This happens when the pkParameter is created from something other than the default set of PK Parameters
            // For example VSS (plasma) for a population which is not calculated from a single curve
            var unit = mergedDimension.UnitOrDefault(pkParameter?.DisplayUnit);

            quantityPKParameter.ValueCache.KeyValues.Each(kv =>
            {
               var row = dataTable.NewRow();
               row[INDIVIDUAL_ID] = kv.Key;
               row[QUANTITY_PATH] = inQuote(quantityPath);
               row[PARAMETER] = inQuote(quantityPKParameter.Name);
               row[VALUE] = mergedDimension.BaseUnitValueToUnitValue(unit, kv.Value).ConvertedTo<string>();
               row[UNIT] = unit.Name;
               dataTable.Rows.Add(row);
            });
         }

         dataTable.EndLoadData();
         return dataTable;
      }

      private string inQuote(string text) => $"\"{text}\"";
   }
}