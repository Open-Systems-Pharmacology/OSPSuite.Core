using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Batch.Mappers
{
   public interface ISimulationResultsToBatchSimulationExportMapper
   {
      BatchSimulationExport MapFrom(IModelCoreSimulation simulation, DataRepository results);
   }

   public class SimulationResultsToBatchSimulationExportMapper : ISimulationResultsToBatchSimulationExportMapper
   {
      private readonly IEntityPathResolver _entityPathResolver;

      public SimulationResultsToBatchSimulationExportMapper(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public BatchSimulationExport MapFrom(IModelCoreSimulation simulation, DataRepository results)
      {
         var simulationExport = new BatchSimulationExport
         {
            Name = simulation.Name,
            Times = timeValuesFrom(results.BaseGrid),
            ParameterValues = parameterValuesFor(simulation.Model),
            AbsTol = simulation.Settings.Solver.AbsTol,
            RelTol = simulation.Settings.Solver.RelTol,
            CheckForNegativeValues = simulation.Settings.Solver.CheckForNegativeValues,
            AutoReduceTolerances = simulation.Settings.Solver.AutoReduceTolerances
         };

         results.AllButBaseGrid().Each(c => simulationExport.OutputValues.Add(quantityResultsFrom(c)));

         return simulationExport;
      }

      private List<ParameterValue> parameterValuesFor(IModel simulationModel)
      {
         return simulationModel.Root.GetAllChildren<IParameter>().Select(p => new ParameterValue
         {
            Path = _entityPathResolver.PathFor(p),
            Value = p.Value
         }).ToList();
      }

      private BatchValues timeValuesFrom(BaseGrid baseGrid) =>
         new BatchValues
         {
            Unit = baseGrid.DisplayUnitName(),
            Values = displayValuesFor(baseGrid),
            Dimension = baseGrid.DimensionName()
         };

      private BatchOutputValues quantityResultsFrom(DataColumn column) =>
         new BatchOutputValues
         {
            Path = consolidatedPathFor(column),
            //ComparisonThreshold should always have a value. If for some reason it is not set, using 0 ensures that value should be compared exactly
            ComparisonThreshold = column.DataInfo.ComparisonThreshold.GetValueOrDefault(0),
            Values = displayValuesFor(column),
            Dimension = column.Dimension.Name,
            Unit = column.DisplayUnitName()
         };

      private static string consolidatedPathFor(DataColumn column)
      {
         var originalPath = column.PathAsString;
         var pathArray = originalPath.ToPathArray();

         //One element at most, return the original path
         if (pathArray.Length <= 1)
            return originalPath;

         //Remove first entry corresponding to simulation name
         return pathArray.Skip(1).ToPathString();
      }

      private float[] displayValuesFor(DataColumn column)
      {
         return column.ConvertToDisplayValues(column.Values).ToArray();
      }
   }
}